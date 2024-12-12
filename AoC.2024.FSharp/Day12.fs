module AoC._2024.FSharp.Day12

open System.Collections.Generic
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils


let offsets = [ (0, 1); (1, 0); (0, -1); (-1, 0) ]

let rec markRegions map regionsMap nextRegion =
    function
    | p :: tail ->
        match Array2D.get2_ regionsMap p with
        | -1 ->
            let rec markAll queue =
                match queue with
                | curr :: qtail when (Array2D.get2_ regionsMap curr) <> -1 -> markAll qtail
                | (x, y) as curr :: qtail ->
                    Array2D.set2 regionsMap curr nextRegion

                    let neig =
                        offsets
                        |> Seq.map (fun (ox, oy) -> (ox + x, oy + y))
                        |> Seq.where (Array2D.isInside2 map)

                    let nextNeig =
                        neig
                        |> Seq.where (Array2D.get2_ map >> (=) map[x, y])
                        |> Seq.where (Array2D.get2_ regionsMap >> (=) -1)
                        |> List.ofSeq

                    markAll (qtail @ nextNeig)
                | _ -> ()

            markAll [ p ]

            markRegions map regionsMap (nextRegion + 1) tail
        | _ -> markRegions map regionsMap nextRegion tail
    | _ -> nextRegion

let calcRegionPrice regionToChar regionsMap isFirst region =
    let points = Array2D.filterArray regionsMap ((=) region) |> List.ofSeq
    let isInside = Array2D.isInside2 regionsMap

    let area = Seq.length points

    let fences =
        points
        |> Seq.collect (fun (x, y) ->
            offsets
            |> Seq.map (fun (ox, oy as o) -> (ox + x, oy + y), o)
            |> Seq.where (fun (p, _) -> (isInside p |> not) || (Array2D.get2_ regionsMap p) <> region))
        |> List.ofSeq

    let perm =
        if isFirst then
            Seq.length fences
        else
            fences
            |> Seq.where (fun ((fx, fy), (ox, _ as o)) ->
                let isVertical = ox = 0
                if isVertical then
                    not (List.contains ((fx - 1, fy), o) fences)
                else
                    not (List.contains ((fx, fy - 1), o) fences))
            // |> Seq.map (Helpers.silent (fun (p, (ox, _)) -> printf $"%A{p}%c{if ox = 0 then 'v' else 'h'} "))
            |> Seq.length

    // printfn ""
    // printfn $"%A{region} %c{regionToChar region} %A{area} %A{perm} %A{area * perm}"

    area * perm


[<DateInfo(2024, 12, AdventParts.All)>]
[<CustomExample("""RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE""",
                "1930",
                "1206")>]
type Day12() =
    inherit AdventSolution()

    member this.printMap(map: 'a array2d) =
        for y in 0 .. this.Input.Height - 1 do
            for x in 0 .. this.Input.Width - 1 do
                printf $"%A{map[x, y]} "

            printfn ""

    member this.Solve isFirst =
        let map = this.Input.SquareMap()
        let regionsMap = this.Input.SquareMap(fun _ -> -1)

        let points =
            seq {
                for y in 0 .. this.Input.Height - 1 do
                    for x in 0 .. this.Input.Width - 1 -> (x, y)
            }
            |> List.ofSeq

        let regionsCount = markRegions map regionsMap 0 points

        let regionsToChar region =
            Array2D.filterArray regionsMap ((=) region) |> Seq.head |> Array2D.get2_ map

        // this.printMap regionsMap
        // this.printMap map

        Seq.init regionsCount id
        |> List.ofSeq
        |> List.map (calcRegionPrice regionsToChar regionsMap isFirst)
        |> List.sum

    [<CustomRun("VVI\nMII\nMII", correct = "74")>]
    override this.SolvePartOne() = this.Solve true

    [<CustomRun("""EEEEE
EXXXX
EEEEE
EXXXX
EEEEE""", correct = "236")>]
    [<CustomRun("""AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA""", correct = "368")>]
    override this.SolvePartTwo() = this.Solve false
