
本次实验需要使用.Net Core 3
请先安装，并安装fslex fsyacc 10.0.0.0版本


Linux下安装 fsharp 

参考 http://10.66.27.234:8090/#/ocaml/install?id=linux-ubuntu-fsharp-fslex-fsyacc-install

window下 安装 fsharp fslex fsyacc

参考 http://10.66.27.234:8090/#/ocaml/install?id=windows

http://10.66.27.234:8090/#/ocaml/install?id=windows-fslexyacc-install


fsi ==> dotnet fsi


A. 生成词法扫描器 和 语法分析器 解释执行 Generating and compiling the lexer and parser for micro-C, and
   loading the interpreter for micro-C.  (Running fsyacc on CPar.fsy
   produces a bunch of shift/reduce conflict warnings, but as far as I
   can see this is due to a bug in fsyacc, which ignores the %nonassoc
   directives on GT LT GE LE).

   fslex  --lexlib Microsoft.FSharp.Text.Lexing  --unicode CLex.fsl
   fsyacc --lexlib Microsoft.FSharp.Text.Lexing --parslib  Microsoft.FSharp.Text.Parsing --module CPar CPar.fsy
   
 
   //linux
   mono /usr/lib/cli/fsharp/fsi.exe -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

   // windows dotnet core 3 
   dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

   //fsi 中输入
   open ParseAndRun;;
   fromFile "ex1.c";;   // 显示 ex1.c的语法树
   run (fromFile "ex1.c") [17];; //解释执行 ex1.c
   run (fromFile "ex11.c") [8];; //解释执行 ex11.c
	
    //编译microc.exe 命令行程序 
   dotnet clean  interpc.fsproj
   dotnet build  interpc.fsproj
   dotnet run -p interpc.fsproj ex11 8

   interpc -> ./bin/Debug/netcoreapp3.0/interpc


   
B. 编译 micro-C编译器，运行编译器 To compile and use the micro-C compiler

   fslex  --lexlib FSharp.Text.Lexing  --unicode CLex.fsl
   fsyacc --lexlib FSharp.Text.Lexing --parslib  FSharp.Text.Parsing --module CPar CPar.fsy
   
   //启动fsi
   dotnet fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Comp.fs ParseAndComp.fs   

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
   dotnet clean  microc.fsproj
   dotnet build  microc.fsproj
   dotnet run -p microc.fsproj
    
   microc ->  /bin/Debug/netcoreapp3.0/microc
    
   
   
C. 编译backwards编译器  To compile and use the backwards (continuation-based) micro-C compiler:

   fslex  --lexlib FSharp.Text.Lexing  --unicode CLex.fsl
   fsyacc --lexlib FSharp.Text.Lexing --parslib  FSharp.Text.Parsing --module CPar CPar.fsy

   fsi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs ParseAndContcomp.fs   

   open ParseAndContcomp;;
   contCompileToFile (fromFile "ex11.c") "ex11.out";;
   #q;;

   
   javac Machine.java
   java Machine ex11.out 8


D. 构建命令行编译器 microcc.exe Build the backwards micro-C compiler as a command-line program microcc

   fslex  --lexlib FSharp.Text.Lexing  --unicode CLex.fsl
   fsyacc --lexlib FSharp.Text.Lexing --parslib  FSharp.Text.Parsing --module CPar CPar.fsy
   
   dotnet clean  microcc.fsproj
   dotnet build  microcc.fsproj
   dotnet run -p microcc.fsproj

   microcc ->  /bin/Debug/netcoreapp3.0/microcc

   microcc ex11.c

   javac Machine.java
   java Machine ex11.out 8
