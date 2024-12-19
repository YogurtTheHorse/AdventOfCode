module AoC._2024.FSharp.Day19

open System
open System.Collections.Generic
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections


let buildsCount parts pattern =
    let cache = Dictionary<int, int64>()

    let rec innBuildsCount i =
        if i < String.length pattern then
            match cache.TryGetValue(i) with
            | true, v -> v
            | false, _ ->
                let v =
                    parts
                    |> List.where (String.startsWithFrom pattern i)
                    |> List.map (fun part -> innBuildsCount (part.Length + i))
                    |> List.sum

                cache.Add(i, v)

                v
        else
            1

    innBuildsCount 0

[<DateInfo(2024, 19, AdventParts.PartTwo)>]
type Day18() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let parts = this.Input.Lines[0].SmartSplit(",") |> List.ofArray

        let patterns =
            Seq.init (this.Input.Height - 2) ((+) 1)
            |> Seq.map (Array.get this.Input.Lines)
            |> List.ofSeq

        Helpers.printan <| buildsCount parts "bggr"

        let counts =
            patterns
            |> List.map (buildsCount parts)
            |> List.map (Helpers.silent Helpers.printan)
            |> List.where (fun x -> x > 0L)

        if isFirst then
            int64 (Seq.length counts)
        else
            Seq.sum counts


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
