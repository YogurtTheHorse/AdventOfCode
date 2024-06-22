module AoC._2022.FSharp.Day1

open AoC.Library.Runner
open AoC.FSharp

[<DateInfo(2022, 1, AdventParts.All)>]
type Day1() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.FullLines
        |> List.ofArray
        |> List.split ""
        |> List.map (fun group ->
            group
            |> List.map int
            |> List.sum
        )
        |> List.max
        :> obj

    override this.SolvePartTwo() =
        this.Input.FullLines
        |> List.ofArray
        |> List.split ""
        |> List.map (fun group ->
            group
            |> List.map int
            |> List.sum
        )
        |> List.sortDescending
        |> List.take 3
        |> List.sum
        :> obj

