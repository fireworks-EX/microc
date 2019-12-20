(* File MicroC/ParseAndRun.fs *)

module ParseAndRun

let fromString = Parse.fromString

let fromFile = Parse.fromFile

let run = Interp.run
