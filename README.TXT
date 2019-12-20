Compiling and loading the micro-C evaluator and parser (MicroC/README.TXT)
--------------------------------------------------------------------------

Archive microc.zip contains the files used in points A, B and C below.


A. Generating and compiling the lexer and parser for micro-C, and
   loading the interpreter for micro-C.  (Running fsyacc on CPar.fsy
   produces a bunch of shift/reduce conflict warnings, but as far as I
   can see this is due to a bug in fsyacc, which ignores the %nonassoc
   directives on GT LT GE LE).

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsi -r FSharp.PowerPack.dll Absyn.fs CPar.fs CLex.fs Parse.fs Interp.fs ParseAndRun.fs

   open ParseAndRun;;
   fromFile "ex1.c";;
   run (fromFile "ex1.c") [17];;
   run (fromFile "ex11.c") [8];;


B. To compile and use the micro-C compiler

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsi -r FSharp.PowerPack.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Comp.fs ParseAndComp.fs   

   open ParseAndComp;;
   compileToFile (fromFile "ex11.c") "ex11.out";;
   #q;;

   javac Machine.java
   java Machine ex11.out 8


C. To compile and use the backwards (continuation-based) micro-C compiler:

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsi -r FSharp.PowerPack.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs ParseAndContcomp.fs   

   open ParseAndContcomp;;
   contCompileToFile (fromFile "ex11.c") "ex11.out";;
   #q;;

   javac Machine.java
   java Machine ex11.out 8


D. Build the backwards micro-C compiler as a command-line program microcc

   fslex --unicode CLex.fsl
   fsyacc --module CPar CPar.fsy
   fsc -r FSharp.PowerPack.dll Absyn.fs CPar.fs CLex.fs Parse.fs Machine.fs Contcomp.fs MicroCC.fs -o microcc.exe

   microcc ex11.c

   javac Machine.java
   java Machine ex11.out 8
