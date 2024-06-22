module AoC._2022.FSharp.Day5

open AoC.Library.Runner
open AoC.FSharp
open FSharpx

type Command = { Amount: int; Src: int; Dst: int }
type Stack = char list

let applyCommand isFirstPart (stacks: Stack list) (command: Command) =
    let toMove, rest = stacks.[command.Src - 1] |> List.splitAt command.Amount

    let toMove' = if isFirstPart then List.rev toMove else toMove
    
    let stacks' =
        stacks
        |> List.mapi (fun i stack ->
            if i = command.Src - 1 then rest
            elif i = command.Dst - 1 then toMove' @ stack
            else stack)

    stacks'
    
let parse lines =
    let cratesInput, commandsInput = List.split "" lines |> Tuple.ofList2
    let stacksCount = String.smartSplit " " cratesInput[^1] |> List.length

    let commands =
        commandsInput
        |> List.map (String.smartSplit " ")
        |> List.map (function
            | [ _; amount; _; from; _; to_ ] ->
                { Amount = int amount
                  Src =  int from
                  Dst = int to_ }
            | _ -> failwith "Invalid input")

    let rows =
        cratesInput[..^1]
        |> List.map (List.ofSeq >> List.chunkBySize 4)
        |> List.map (
            List.map
            <| function
                | [ '['; c; ']' ] -> Some c
                | [ '['; c; ']'; ' ' ] -> Some c
                | _ -> None
        )

    let stacks =
        [ for i in 0 .. (stacksCount - 1) -> rows |> List.map (fun row -> row[i]) |> List.choose id ]
        
    (stacks, commands)

[<DateInfo(2022, 5, AdventParts.PartTwo)>]
type Day5() =
    inherit AdventSolution()
 
    override this.SolvePartOne() =
        let stacks, commands = parse this.Input.FullLines[..^1]
        let resultStacks = List.fold (applyCommand true) stacks commands

        resultStacks |> List.map (List.head >> string) |> String.concat "" :> obj


    override this.SolvePartTwo() = 
        let stacks, commands = parse this.Input.FullLines[..^1]
        let resultStacks = List.fold (applyCommand false) stacks commands

        resultStacks |> List.map (List.head >> string) |> String.concat "" :> obj
