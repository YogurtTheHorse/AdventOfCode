module AoC._2024.FSharp.Day9

open AoC.Library.Runner

type BlockType =
    | Empty
    | File

type Block =
    { Id: int
      Type: BlockType
      Length: int // ignored in first part
      Pos: int }

let fileMatch block =
    match block with
    | { Type = File } -> true
    | _ -> false

let rec buildBlocks isFirst currPos currId isFile leftNums =
    match leftNums with
    | [] -> Seq.empty
    | len :: tail ->
        seq {
            if isFirst then
                for _ in 0 .. len - 1 do
                    yield
                        { Id = currId
                          Length = 1
                          Pos = currPos + len
                          Type = if isFile then File else Empty }
            elif len > 0 then
                yield
                    { Id = currId
                      Length = len
                      Pos = currPos
                      Type = if isFile then File else Empty }

            yield! (buildBlocks isFirst (currPos + len) (if isFile then currId + 1 else currId) (not isFile) tail)
        }

let removeBlock block blocks =
    let simpleClear = List.where ((<>) block) blocks

    let rec cleanUp state =
        function
        | { Type = Empty; Length = l1; Pos = p } :: { Type = Empty; Length = l2 } :: tail ->
            let newEmpty =
                { Type = Empty
                  Length = l1 + l2 + block.Length
                  Pos = p
                  Id = 0 }

            cleanUp (newEmpty :: state) tail
        | head :: tail -> cleanUp (head :: state) tail
        | [] -> List.rev state
        
    cleanUp [] simpleClear

let printBlocks blocks =
    let s =
        blocks
        |> List.collect (fun b -> [ for _ in 0 .. b.Length - 1 -> b ])
        |> List.map (function
            | { Type = Empty } -> "."
            | { Id = i } -> string i)
        |> String.concat ""

    printfn $"%A{s}"



let sortedFirst blocks =
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

    let filesCount = blocks |> Seq.where fileMatch |> Seq.map _.Length |> Seq.sum

    sorted filesCount blocks (List.rev blocks |> List.where fileMatch) []

let sortedSecond blocks =
    let currId = blocks |> Seq.map _.Id |> Seq.max

    let rec sorted currId blocks =
        if currId <= 0 then
            blocks
        else
            let currBlockId =
                blocks |> List.findIndexBack (fun b -> b.Id = currId && b.Type = File)

            let currBlock = blocks[currBlockId]

            let firstAvailableId =
                List.tryFindIndex (fun b -> b.Type = Empty && b.Length >= currBlock.Length && b.Pos < currBlock.Pos) blocks

            match firstAvailableId with
            | Some i when blocks[i].Length = currBlock.Length ->
                let newBlocks =
                    blocks
                    |> List.removeAt currBlockId
                    |> List.removeAt i
                    |> List.insertAt i { currBlock with Pos = blocks[i].Pos }
                
                sorted (currId - 1) newBlocks
            | Some i ->
                let empty = blocks[i]
                let newblocks =
                    blocks
                    |> List.removeAt currBlockId
                    |> List.removeAt i
                    |> List.insertAt i { currBlock with Pos = empty.Pos }
                    |> List.insertAt
                        (i + 1)
                        { empty with
                            Pos = empty.Pos + currBlock.Length
                            Length = empty.Length - currBlock.Length }
                    
                sorted (currId - 1) newblocks
            | _ -> sorted (currId - 1) blocks

    sorted currId blocks



[<DateInfo(2024, 9, AdventParts.All)>]
type Day9() =
    inherit AdventSolution()

    member this.Solve isFirst (sorted: Block list -> Block list) =
        let line = this.Input.Lines[0] |> Seq.map (string >> int) |> List.ofSeq

        let blocks = buildBlocks isFirst 0 0 true line |> List.ofSeq
        let sorted = sorted blocks

        // printBlocks blocks
        // printBlocks sorted

        let hash =
            sorted
            |> List.where fileMatch
            |> List.collect (fun b -> [ for i in 0 .. b.Length - 1 -> int64 b.Id * int64 (b.Pos + i) ])
            |> List.sum

        hash

    override this.SolvePartOne() =
        this.Solve true sortedFirst

    override this.SolvePartTwo() =
        this.Solve false sortedSecond
