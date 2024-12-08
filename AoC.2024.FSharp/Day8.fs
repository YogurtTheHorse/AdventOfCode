module AoC._2024.FSharp.Day8

open AoC.Library.Runner

let pairs list =
    seq {
        for i in 0 .. List.length list - 2 do
            for j in i + 1 .. List.length list - 1 do
                yield (list.[i], list.[j])
    }

let isInside w h (x, y) =
    x >= 0 && y >= 0 && x < w && y < h

let generateAntinodes (fx, fy) (dx, dy) =
    Seq.initInfinite (fun index -> fx + dx * (0 + index), fy + dy * (0 + index))

let getAntiNodes w h multiple nodes =
    nodes
    |> pairs
    |> Seq.collect (fun (a, b) ->
        let ax, ay = a
        let bx, by = b
        let ox, oy = (bx - ax, by - ay)

        if multiple then
            let aseq = generateAntinodes a (-ox, -oy) |> Seq.takeWhile (isInside w h)
            let bseq = generateAntinodes b (ox, oy) |> Seq.takeWhile (isInside w h)
            
            Seq.concat [aseq; bseq]
        else
            [ (ax - ox, ay - oy); (bx + ox, by + oy) ])
    |> Seq.filter (isInside w h)

[<DateInfo(2024, 8, AdventParts.All)>]
type Day8() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let map = this.Input.SquareMap()
        let w = this.Input.Width
        let h = this.Input.Height

        seq {
            for x in 0 .. (Array2D.length1 map - 1) do
                for y in 0 .. (Array2D.length2 map - 1) -> if map[x, y] <> '.' then Some(map[x, y], (x, y)) else None
        }
        |> Seq.choose id
        |> Seq.groupBy fst
        |> Seq.map (snd >> Seq.map snd >> List.ofSeq)
        |> Seq.collect (getAntiNodes w h (not isFirst))
        |> Seq.distinct
        |> Seq.length


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
