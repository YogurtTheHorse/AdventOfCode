module AoC._2024.FSharp.Day9

open AoC.Library.Runner
open AoC.FSharp
open AoC.Library.Utils

type BlockType =
    | Empty
    | File

type Block = { Id: int; Pos: int; Type: BlockType }

let fileMatch =
    function
    | { Type = File } -> true
    | _ -> false

let rec buildBlocks currId currPos isFile =
    function
    | [] -> Seq.empty
    | len :: tail ->
        seq {
            for i in 0 .. len - 1 do
                yield
                    { Id = currId
                      Pos = (i + currPos)
                      Type = if isFile then File else Empty }

            yield! (buildBlocks (if isFile then currId + 1 else currId) (currPos + len) (not isFile) tail)
        }

let rec sorted filesLeft blocks revBlocks state =
    if filesLeft <= 0 then
        List.rev state
    else
        match blocks with
        | { Type = File } as file :: tail -> sorted (filesLeft - 1) tail revBlocks (file :: state)
        | { Type = Empty } :: tail ->
            match revBlocks with
            | head :: revTail -> sorted (filesLeft - 1) tail revTail (head :: state)
            | _ -> failwith "AAAA"
        | _ -> failwith "AAAAAAA"

[<DateInfo(2024, 9, AdventParts.PartOne)>]
type Day9() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let line = this.Input.Lines[0] |> Seq.map (string >> int) |> List.ofSeq

        let blocks = buildBlocks 0 0 true line |> List.ofSeq
        let revBlocks = List.rev blocks |> List.where fileMatch
        let filesCount = blocks |> Seq.where fileMatch |> Seq.length

        
        let sorted = sorted filesCount blocks revBlocks [] |> List.map (_.Id)
        
        // let d1 = blocks |> List.map (function | {Type = Empty} -> "." | {Id = i} -> string i) |> String.concat ""
        // let d2 = sorted |> Seq.map string |> Array.ofSeq |> String.concat ""
        //
        // printf $"%A{d1}\n%A{d2}\n"

        let hash =
            sorted |> List.indexed |> List.map (fun (i, v) -> int64 i * int64 v) |> List.sum

        hash



    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
