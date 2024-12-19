module AoC._2024.FSharp.Day13

open System.Collections.Generic
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

let splitBy len list =
    let l = List.length list
    let n = len
    let r = l % (l / len)

    List.append
        [ (List.take (n + r) list) ]
        (List.unfold
            (fun rest ->
                match rest with
                | [] -> None
                | _ ->
                    let taken = min n (List.length rest)
                    Some(List.take taken rest, List.skip taken rest))
            (List.skip (n + r) list))

let parseTask isFirst raw =
    let parseV (btn: string) =
        let xs, ys =
            btn.SmartSplit(" ")
            |> Array.rev
            |> Array.take 2
            |> Array.rev
            |> Array.map (_.Trim(',').Trim('X').Trim('Y').Trim('='))
            |> Tuple.ofArray2

        int64 xs, int64 ys

    match raw with
    | [ aStr; bStr; prizeStr ] ->
        let a = parseV aStr
        let b = parseV bStr
        let px, py = parseV prizeStr

        let p =
            if isFirst then
                px, py
            else
                10000000000000L + px, 10000000000000L + py

        a, b, p
    | _ -> failwith "Whhaar"

let third (_, _, c) = c

let prizePriceOld isFirst (ax: int64, ay: int64) (bx: int64, by: int64) (px: int64, py: int64) =
    let maxACount = int (max (px / ax) (py / ay))

    Seq.init (maxACount + 1) int64
    |> Seq.map (fun ai -> ai, (px - ax * ai) / bx)
    |> Seq.where (fun (ai, bi) -> not isFirst || (ai <= 100 && bi <= 100))
    |> Seq.where (fun (ai, bi) ->
        let x = (px = ax * ai + bx * bi)
        let y = (py = ay * ai + by * bi)

        x && y)
    |> Seq.map (fun (ai, bi) -> ai * 3L + bi)
    |> Seq.tryHead
    |> Option.defaultValue 0

let prizePrice isFirst (ax: int64, ay: int64) (bx: int64, by: int64) (px: int64, py: int64) =
    let det = ax * by - ay * bx
    let ab = (by * px - bx * py)
    let bb = (-ay * px + ax * py)
    
    if ab % det <> 0 || bb % det <> 0 then
        0L
    else
        let a = ab / det
        let b = bb / det
        
        // printfn  $"%A{a} %A{b}"
        
        if (not isFirst) || (a <= 100 && b <= 100) then 
            a * 3L + b
        else
            0L

[<DateInfo(2024, 13, AdventParts.PartTwo)>]
type Day13() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let tasks = splitBy 3 (List.ofSeq this.Input.Lines)

        tasks
        |> List.map (parseTask isFirst)
        |> List.map (Helpers.t3 (prizePrice isFirst))
        |> List.sum


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
