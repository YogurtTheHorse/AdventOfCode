module AoC._2024.FSharp.Day21

open System
open System.Collections.Generic
open System.Text
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections

let offsets = [ (0, 1); (1, 0); (0, -1); (-1, 0) ] |> List.map Point.ofTuple

let positionsNum =
    [ 'A', (2, 3)
      '0', (1, 3)
      '1', (0, 2)
      '2', (1, 2)
      '3', (2, 2)
      '4', (0, 1)
      '5', (1, 1)
      '6', (2, 1)
      '7', (0, 0)
      '8', (1, 0)
      '9', (2, 0) ]
    |> List.map (fun (c, p) -> c, Point.ofTuple p)
    |> Map

let positionsDir =
    [ 'A', (2, 0); '^', (1, 0); '<', (0, 1); 'v', (1, 1); '>', (2, 1) ]
    |> List.map (fun (c, p) -> c, Point.ofTuple p)
    |> Map

type Cache = Dictionary<PointBase<int> * PointBase<int>, string>


let getPath (positions: Map<char, PointBase<int>>) emptyPos currPos nextPos =
    let struct (mn, mx) = Point.bounds currPos nextPos

    let h = nextPos.X - currPos.X
    let v = nextPos.Y - currPos.Y

    let vc = if v > 0 then "v" else "^"
    let hc = if h > 0 then ">" else "<"

    let vs = String.init (abs v) (fun _ -> vc)
    let hs = String.init (abs h) (fun _ -> hc)

    let isGoingLeft = h < 0
    let leftIsBlocked = Point.isInBounds2 mn mx emptyPos

    if isGoingLeft <> leftIsBlocked then
        hs + vs + "A"
    else
        vs + hs + "A"

let cachedGetPath (cache: Cache) (positions: Map<char, PointBase<int>>) emptyPos currPos nextPos =
    if nextPos = currPos then
        "A"
    else
        match cache.TryGetValue((currPos, nextPos)) with
        | true, s -> s
        | false, _ ->
            let v = getPath positions emptyPos currPos nextPos
            cache.Add((currPos, nextPos), v)

            v

let cache = Cache()

let rec commands (positions: Map<char, PointBase<int>>) emptyPos currPos path leftChars =
    match leftChars with
    | [] -> path
    | head :: tail ->
        let nextPos = positions[head]
        let npath = path + (cachedGetPath cache positions emptyPos currPos nextPos)

        commands positions emptyPos nextPos npath tail


[<DateInfo(2024, 21, AdventParts.PartTwo)>]
[<CustomExample("029A\n980A\n179A\n456A\n379A", "126384")>]
type Day21() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let seqs = this.Input.Lines |> List.ofArray

        let numc v =
            (commands positionsNum (PointBase(0, 3)) (PointBase(2, 3)) "") (List.ofSeq v)

        let dirc v =
            (commands positionsDir (PointBase(0, 0)) (PointBase(2, 0)) "") (List.ofSeq v)

        let dirrobots = if isFirst then 2 else 25

        let rec applyDir cnt v =
            if cnt > 0 then
                Helpers.printan cnt
                applyDir (cnt - 1) (dirc v)
            else
                v

        seqs
        |> List.map (fun s -> int s[.. s.Length - 2], s |> numc |> applyDir dirrobots |> String.length)
        |> List.map (Helpers.silent (snd >> Helpers.printan))
        |> List.map (Helpers.t2 (*))
        |> List.sum


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
