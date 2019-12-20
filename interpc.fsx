
#r "FsLexYacc.Runtime.dll";;
#load "Absyn.fs" "CPar.fs" "CLex.fs" "Parse.fs" "Machine.fs" "Interp.fs" ;;
let fromFile = Parse.fromFile;;
let run = Interp.run;;
let args = System.Environment.GetCommandLineArgs();;

let _ = printf "Micro-C interpreter v 1.0.0.1 of 2017-12-2\n";;

let _ = 
   if args.Length > 1 then
      let source = args.[1]
      let inputargs = Array.splitAt 2 args |> snd |> (Array.map int)|> Array.toList

      printf "interpreting %s ...inputargs:%A\n" source  inputargs;
      try ignore (         
          let ex = fromFile source;
          run ex inputargs)
      with Failure msg -> printf "ERROR: %s\n" msg
   else
      printf "Usage: interpc.exe <source file> <args>\n";;

#q;;