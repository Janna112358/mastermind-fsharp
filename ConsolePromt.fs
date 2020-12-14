namespace Mastermind

open System

module Prompt = 

    let rec tryGetInput checkFun tries (message : string) = 

        printfn "%s" message 
        let input = Console.ReadLine()

        if (checkFun input) then
            Some input
        else
            if tries <= 1 then
                printfn "Did not get a valid input (last input: %s)" input
                None
            else
                tryGetInput checkFun (tries - 1) message

    let checkInt (input : string) = 
        match Int32.TryParse input with
            | (true, n) -> true
            | (false, _) -> false

    let tryGetInt maxTries message = 
        let input = tryGetInput checkInt maxTries message
        match input with
        | Some s -> Some(int s)
        | None -> None