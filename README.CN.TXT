Compiling and loading the micro-C evaluator and parser (MicroC/README.TXT)
--------------------------------------------------------------------------

Archive microc.zip contains the files used in points A, B and C below.

Linux下fsi为

mono /usr/lib/cli/fsharp/fsi.exe 

window 安装
https://dotnet.microsoft.com/download/dotnet-core/3.0

>dotnet  --list-sdks
3.0.100 [C:\Program Files\dotnet\sdk]
表示安装正确


A. 生成词法扫描器 和 语法分析器 解释执行 Generating and compiling the lexer and parser for micro-C, and
   loading the interpreter for micro-C.  (Running fsyacc on CPar.fsy
   produces a bunch of shift/reduce conflict warnings, but as far as I
   can see this is due to a bug in fsyacc, which ignores the %nonassoc
   directives on GT LT GE LE).

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   
   //启动fsi
   fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

   //linux
   mono /usr/lib/cli/fsharp/fsi.exe -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

   // dotnet core 3 
   dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs


   //fsi 中输入
   open ParseAndRun;;
   fromFile "ex1.c";;   // 显示 ex1.c的语法树
   run (fromFile "ex1.c") [17];; //解释执行 ex1.c
   run (fromFile "ex11.c") [8];; //解释执行 ex11.c
	
   //或者用fsharp脚本, 3为 ex9 的参数.
   fsi InterpC.fsx ex9.c 3  

    //编译microc.exe 命令行程序 
   //注意 microc.exe.config文件里面对老版本 Fsharp.Core的重定向
   fsc microc.fsx
   microc.exe ex9.c
   
   
B. 编译 micro-C编译器，运行编译器 To compile and use the micro-C compiler

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   
   //启动fsi
   fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Comp.fs ParseAndComp.fs   

   //fsi 中输入，运行编译器
   open ParseAndComp;;
   compileToFile (fromFile "ex11.c") "ex11.out";;  //生成机器码ex11.out
   #q;;

   
   //执行生成的 机器代码 “ex11.out”
   javac Machine.java
   java Machine ex11.out 8

   // fsi 中运行
   #time "on";;  // 打开时间跟踪
   // 参考A. 中的命令 比较下解释执行解释执行 与 编译执行 ex11.c 的速度

   //运行下面的命令 查看 fac 0 , fac 3 的栈帧 
   //理解栈式虚拟机执行流程
   
   java Machinetrace ex9.out 0
   java Machinetrace ex9.out 3
  
   //编译microc.exe 命令行程序 
   //注意 microc.exe.config文件里面对老版本 Fsharp.Core的重定向
   fsc microc.fsx
   microc.exe ex9.c
    
   
   
C. 编译backwards编译器  To compile and use the backwards (continuation-based) micro-C compiler:

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs ParseAndContcomp.fs   

   open ParseAndContcomp;;
   contCompileToFile (fromFile "ex11.c") "ex11.out";;
   #q;;

   javac Machine.java
   java Machine ex11.out 8


D. 构建命令行编译器 microcc.exe Build the backwards micro-C compiler as a command-line program microcc

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsc -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs MicroCC.fs -o microcc.exe

   microcc ex11.c

   javac Machine.java
   java Machine ex11.out 8
