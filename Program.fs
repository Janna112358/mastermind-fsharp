
open System
open Mastermind

[<EntryPoint>]
let main argv =

    try
        Game.FromPromt
        // |> Game.printSecrets
        |> Game.play
        0
    with
    | :? FormatException as e ->
        printfn "Error: %s" e.Message
        printfn "Input was not in the expected format"
        1
    | e ->
        printfn "Unexpected error: %s" e.Message 
        2
