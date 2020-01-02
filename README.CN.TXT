//生成词法扫描器 和 语法分析器
fslex --unicode CLex.fsl
fsyacc --module CPar CPar.fsy
   
//启动fsi
fsharpi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

//运行解释器
open ParseAndRun;;

run (fromFile "++--.c") [];;
run (fromFile "+=-=.c") [];;
run (fromFile "for.c") [];;
run (fromFile "test.c") [];;

#q;;

//启动fsi
fsharpi -r FsLexYacc.Runtime.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs ParseAndContcomp.fs     

//运行编译器
open ParseAndContcomp;;

contCompileToFile (fromFile "++--.c") "++--.out";;
contCompileToFile (fromFile "+=-=.c") "+=-=.out";;
contCompileToFile (fromFile "test.c") "test.out";;
contCompileToFile (fromFile "for.c") "for.out";;

#q;;

//生成虚拟机
javac Machine.java

//执行生成的 机器代码
java Machine ++--.out
java Machine +=-=.out
java Machine test.out
java Machine for.out

//运行下面的命令 查看栈帧
java Machinetrace ++--.out
java Machinetrace +=-=.out
java Machinetrace test.out
java Machinetrace for.out