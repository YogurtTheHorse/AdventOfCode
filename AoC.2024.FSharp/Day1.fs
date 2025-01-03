module AoC._2024.FSharp.Day1

open AoC.Library.Runner
open AoC.FSharp

[<DateInfo(2024, 1, AdventParts.All)>]
type Day1() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.Lines
        |> List.ofArray
        |> List.map (String.smartSplit " " >> List.map int >> Tuple.ofList2)
        |> List.unzip
        |> (fun (a, b) -> List.sort a, List.sort b)
        |> List.zip
        |> List.map (Helpers.t2 (-) >> abs)
        |> List.sum
        :> obj


    override this.SolvePartTwo() =
        this.Input.Lines
        |> List.ofArray
        |> List.map (String.smartSplit " " >> List.map int >> Tuple.ofList2)
        |> List.unzip
        |> fun (a, b) -> a |> List.map (fun aa -> (List.count b aa) * aa) |> List.sum
        :> obj
