module AoC._2022.FSharp.Day4

open AoC.Library.Runner
open AoC.FSharp

[<DateInfo(2022, 4, AdventParts.PartTwo)>]
type Day4() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.Lines
        |> Seq.map (
            String.smartSplit ","
            >> (List.map (String.smartSplit "-" >> List.map int >> Tuple.ofList2))
            >> Tuple.ofList2
        )
        |> Seq.where (fun ((fb, fe), (sb, se)) -> (fb <= sb && fe >= se) || (fb >= sb && fe <= se))
        |> Seq.length
        :> obj


    override this.SolvePartTwo() = 
        this.Input.Lines
        |> Seq.map (
            String.smartSplit ","
            >> (List.map (String.smartSplit "-" >> List.map int >> Tuple.ofList2))
            >> Tuple.ofList2
        )
        |> Seq.where (fun ((fb, fe), (sb, se)) -> (fb <= sb && fe >= se) || (fb >= sb && fe <= se) || (fb >= sb && fb <= se) || (fe >= sb && fe <= se))
        |> Seq.length
        :> obj
    