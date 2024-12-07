module AoC._2024.FSharp.Day7

open AoC.Library.Runner
open AoC.FSharp
open AoC.Library.Utils

let canCalc canConcat res operands =
    let rec tryCalc curr =
        function
        | _ when curr > res -> false
        | [] -> curr = res
        | head :: tail ->
            (tryCalc (curr * head) tail)
            || (tryCalc (curr + head) tail)
            || (canConcat && tryCalc (int64 (string curr + string head)) tail)

    match operands with
    | fst :: tail -> tryCalc fst tail
    | _ -> failwith "incorrect operands"


[<DateInfo(2024, 7, AdventParts.All)>]
type Day7() =
    inherit AdventSolution()

    member this.Solve isFirst =
        this.Input.Lines
        |> Array.map (_.SmartSplit(":"))
        |> Array.map (fun arr -> (int64 arr[0], arr[1] |> _.SmartSplit(" ") |> Array.map int64 |> List.ofArray))
        |> Array.where (Helpers.t2 (canCalc (not isFirst)))
        |> Array.map fst
        |> Array.sum
        :> obj

    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
