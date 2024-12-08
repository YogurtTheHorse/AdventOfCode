module AoC._2024.FSharp.Day5

open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

let isGoodManual rules pages =
    pages
    |> List.mapi (fun i p ->
        rules
        |> List.where (fst >> (=) p)
        |> List.map snd
        |> List.choose (fun s -> List.tryFindIndex ((=) s) pages)
        |> List.forall ((<) i))
    |> List.forall id


let getSorted rules =
    List.sortWith (fun x y ->
        rules
        |> List.tryFind (fun (xx, yy) -> xx = x && yy = y || xx = y && yy = x)
        |> Option.map (fun (xx, _) -> if xx = x then 1 else -11)
        |> Option.defaultValue 0)

[<DateInfo(2024, 5, AdventParts.All)>]
type Day5() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let rulesRaw, checkRaw =
            this.Input.Raw.SmartSplit("\n\n")
            |> Array.map (_.SmartSplit("\n"))
            |> Array.map List.ofArray
            |> Tuple.ofArray2

        let rules =
            rulesRaw
            |> List.map (fun x -> x.SmartSplit("|") |> Array.map int)
            |> List.map Tuple.ofArray2

        let check =
            checkRaw
            |> List.map (fun x -> x.SmartSplit(",") |> Array.map int |> List.ofArray)

        if isFirst then
            check
            |> List.where (isGoodManual rules)
            |> List.map (fun m -> m[m.Length / 2])
            |> List.sum
            :> obj
        else
            check
            |> List.where (isGoodManual rules >> not)
            |> List.map (getSorted rules)
            |> List.map (fun m -> m[m.Length / 2])
            |> List.sum
            :> obj


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
