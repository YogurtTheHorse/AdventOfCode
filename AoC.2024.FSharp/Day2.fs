module AoC._2024.FSharp.Day2

open AoC.Library.Runner
open AoC.FSharp

type IsGoodSecond =
    | Bad
    | GoodOriginal
    | GoodRemoved of int

let differences lst =
    lst
    |> List.mapi (fun i x -> if i = 0 then None else Some(x - lst.[i - 1]))
    |> List.choose id

let isGoodLevel (nums: int list) =
    let diffs = differences nums
    let correctSign = sign diffs[0]
    let signs = List.forall (fun d -> sign d = correctSign) diffs
    let inRanges = List.forall (fun d -> d >= -3 && d <= 3 && d <> 0) diffs

    signs && inRanges && correctSign <> 0
    
let isGoodSecond (nums: int list) =
    if isGoodLevel nums then
        GoodOriginal
    else
        let count = List.length nums
        
        let removeNthElement n =
            nums
            |> List.mapi (fun i x -> (i, x))
            |> List.filter (fun (i, _) -> i <> n)
            |> List.map snd 
        
        let results =
            seq { for i in 0..count - 1 -> i }
            |> Seq.map removeNthElement
            |> Seq.mapi (fun i l -> if isGoodLevel l then GoodRemoved i else Bad)
            |> Seq.where (function | Bad -> false | _ -> true)
            |> List.ofSeq
            
        if List.isEmpty results then
            Bad
        else
            List.head results
        
        

[<DateInfo(2024, 2, AdventParts.PartTwo)>]
type Day2() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        this.Input.Lines
        |> List.ofArray
        |> List.map (String.smartSplit " ")
        |> List.map (List.map int)
        |> List.where isGoodLevel
        |> List.length
        :> obj


    override this.SolvePartTwo() =
        this.Input.Lines
        |> List.ofArray
        |> List.map (String.smartSplit " ")
        |> List.map (List.map int)
        |> List.map isGoodSecond
        |> Helpers.silent (printf "%A\n")
        |> List.where (function | Bad -> false | _ -> true)
        |> List.length
        :> obj
