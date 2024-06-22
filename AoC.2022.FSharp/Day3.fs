module AoC._2022.FSharp.Day3

open AoC.FSharp
open AoC.Library.Runner
open FSharpx

let find (a: string) b = a |> Seq.find (String.containsChar2 b)

let find2 (a: string) b c = a |> Seq.find (fun ch -> String.containsChar2 b ch && String.containsChar2 c ch)

let toNum (c: char) =
    if int c >= int 'a' then (1 + int c - int 'a') else (27 + (int c - int 'A'))

[<DateInfo(2022, 3, AdventParts.PartTwo)>]
type Day3() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.Lines
        |> Seq.map String.halves
        |> Seq.map (Helpers.t2 find)
        |> Seq.map (Helpers.silent this.WriteLine)
        |> Seq.map toNum
        |> Seq.map (Helpers.silent this.WriteLine)
        |> Seq.sum
        :> obj


    override this.SolvePartTwo() = 
        this.Input.Lines
        |> Seq.chunkBySize 3
        |> Seq.map Tuple.ofArray3
        |> Seq.map (Helpers.t3 find2)
        |> Seq.map toNum
        |> Seq.sum
        :> obj
