namespace Mastermind

open System
open Mastermind

type Game = 
    {
        CodeLen : int
        SecretCode : string
        GuessHistory : list<string>
    }

module Game = 

    let FromPromt = 

        printfn "Welcome to Mastermind!"
        let msg = "Please enter an integer code length, then press Enter: "            
        let inputInt = Prompt.tryGetInt 3 msg
        let codeLen = 
            match inputInt with
                | Some n -> 
                    if n > 10 then
                        printfn "Code length %i too big, defaulting to 10" n
                        10
                    elif n < 1 then 
                        printfn "Code length %i too small, defaulting to 1" n
                        1
                    else
                        n
                | None ->
                    raise <| FormatException "Did not get a valid input"

        let code = Code.randomCode codeLen
        {CodeLen = codeLen; SecretCode = code; GuessHistory = []}

    let printSecrets (game : Game) = 
        printfn "Secret code of length %i is %s" game.CodeLen game.SecretCode
        printfn "Guesses so far: %A" game.GuessHistory
        game

    let takeGuess (game : Game) =
        let msg = sprintf "Enter you %i-digit guess: " game.CodeLen
        let input = Prompt.tryGetInput (fun s -> 
            if s = "ShowMeYourSecrets" then
                game |> printSecrets |> ignore
                false
            else
                s.Length = game.CodeLen) 3 msg
        let guess = 
            match input with
                | Some g -> 
                    //printfn "your guess is %s" g
                    g
                | None ->
                    raise <| (FormatException "invalid input")

        {game with 
            GuessHistory = guess :: game.GuessHistory}

    let scoreLast (game : Game) = 
        let lCode = Code.toList game.SecretCode
        let lGuess = 
            match game.GuessHistory with
            | head :: tail -> Code.toList head
            | [] -> raise <| FormatException "No guesses yet"

        let mutable countCorrect = 0
        let mutable countIn = 0

        let digits = [0 .. 9]
        for i in digits do
            let inCode = 
                lCode
                |> List.filter (fun e -> e = i)
                |> List.length
            let inGuess = 
                lGuess
                |> List.filter (fun e -> e = i)
                |> List.length 
            
            let inBoth = List.min [inCode; inGuess]
            countIn <- countIn + inBoth

        for i in [0 .. (game.CodeLen - 1)] do
            if lCode.[i] = lGuess.[i] then
                countCorrect <- countCorrect + 1
                countIn <- countIn - 1

        printfn "Score %i %i (correct in the right place, then correct in the wrong place)" countCorrect countIn
        
        (countCorrect, countIn)

    let rec play (game : Game) = 
        let game' = 
            takeGuess game
        
        let score = 
            scoreLast game'

        match score with
        | (x, _) ->
            if x = game.CodeLen then
                printfn "Hooray! You've won the game in %i gueses" (List.length game'.GuessHistory)
            else
                play game'


    