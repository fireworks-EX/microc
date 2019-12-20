(* Lexing and parsing of micro-C programs using fslex and fsyacc *)

module Parse

open System
open System.IO
open System.Text
open Microsoft.FSharp.Text
open Absyn

(* Plain parsing from a string, with poor error reporting *)

let fromString (str : string) : program =
    let lexbuf = Lexing.LexBuffer<char>.FromString(str)
    try 
      CPar.Main CLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s near line %d, column %d\n" 
                  (exn.Message) (pos.Line+1) pos.Column
             
(* Parsing from a file *)

let fromFile (filename : string) =
    use reader = new StreamReader(filename)
    let lexbuf = Lexing.LexBuffer<char>.FromTextReader reader
    try 
      CPar.Main CLex.Token lexbuf
    with 
      | exn -> let pos = lexbuf.EndPos 
               failwithf "%s in file %s near line %d, column %d\n" 
                  (exn.Message) filename (pos.Line+1) pos.Column

