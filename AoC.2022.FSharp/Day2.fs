module AoC._2022.FSharp.Day2

open AoC.FSharp
open AoC.Library.Runner
open FSharpx


type private RPS =
    | Rock
    | Paper
    | Scissors

type private RPSResult =
    | Win
    | Lose
    | Draw

module private RPS =
    let parse =
        function
        | "X"
        | "A" -> Rock
        | "Y"
        | "B" -> Paper
        | "Z"
        | "C" -> Scissors
        | _ -> failwith "Invalid input"

    let parseResult =
        function
        | "X" -> Lose
        | "Y" -> Draw
        | "Z" -> Win
        | _ -> failwith "Invalid input"

    let compare a b =
        match a, b with
        | Rock, Scissors
        | Paper, Rock
        | Scissors, Paper -> Win
        | Rock, Paper
        | Paper, Scissors
        | Scissors, Rock -> Lose
        | _ -> Draw

    let find r a =
        match (a, r) with
        | Rock, Win -> Paper
        | Paper, Win -> Scissors
        | Scissors, Win -> Rock
        | Rock, Lose -> Scissors
        | Paper, Lose -> Rock
        | Scissors, Lose -> Paper
        | _, Draw -> a



    let toNumber =
        function
        | Rock -> 1
        | Paper -> 2
        | Scissors -> 3

    let resToNumber =
        function
        | Win -> 6
        | Draw -> 3
        | Lose -> 0

[<DateInfo(2022, 2, AdventParts.All)>]
type Day2() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.Lines
        |> Array.map (String.splitChar [| ' ' |] >> Array.map RPS.parse >> Tuple.ofArray2)
        |> Array.map (fun (a, b) -> ((RPS.compare b a) |> RPS.resToNumber) + (RPS.toNumber b))
        // |> Array.map (fun x ->
        //     printfn $"%d{x}"
        //     x)
        |> Array.sum
        :> obj


    override this.SolvePartTwo() =
        this.Input.Lines
        |> Array.map (String.splitChar [| ' ' |] >> Tuple.ofArray2)
        |> Array.map (fun (a, r) -> (RPS.parse a, RPS.parseResult r))
        |> Array.map (fun (a, r) -> (r |> RPS.resToNumber) + (a |> RPS.find r |> RPS.toNumber))
        |> Array.sum
        :> obj
