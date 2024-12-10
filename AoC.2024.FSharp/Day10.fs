module AoC._2024.FSharp.Day10

open AoC.Library.Runner
open AoC.FSharp
open AoC.Library.Utils

let offsets = [
    (0, 1)
    (1, 0)
    (0, -1)
    (-1, 0)
]

let countFinishes map isFirst start =
    let isInside (x, y) =
        x >= 0 && y >= 0 && x < Array2D.length1 map && y < Array2D.length2 map
        
    let rec findFinishes (x, y) =
        if map[x, y] = 9 then
            Seq.singleton (x, y)
        else
            let next = map[x, y] + 1
            
            offsets
            |> Seq.map (fun (ox, oy) -> (x + ox, y + oy))
            |> Seq.where isInside
            |> Seq.where (Array2D.get2_ map >> (=) next)
            |> Seq.collect findFinishes
    
    let finishes = findFinishes start
    
    if isFirst then
        finishes
        |> Seq.distinct
        |> Seq.length
    else
        finishes
        |> Seq.length
            
    

[<DateInfo(2024, 10, AdventParts.All)>]
[<CustomExample("89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732", "36", "81")>]
type Day10() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let map = this.Input.SquareMap(string >> int)
        let starts = Array2D.filterArray map ((=) 0)
        
        starts
        |> Seq.map (countFinishes map isFirst)
        |> Helpers.silent (List.ofSeq >> printfn "%A")
        |> Seq.sum

    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
