module AoC._2023.FSharp.Day23

open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

type Point = PointBase<int>

type Cell =
    | Wall
    | Slope of Direction
    | Empty

let mapCell =
    function
    | '#' -> Wall
    | '>' -> Slope(Direction.Right)
    | 'v' -> Slope(Direction.Down)
    | '<' -> Slope(Direction.Left)
    | _ -> Empty

let neig map x y p2 =
    Array2D.get map x y
    |> function
        | Slope(dir) when not p2 -> Seq.singleton dir
        | Wall -> Seq.empty
        | _ -> Seq.ofList [ Direction.Up; Direction.Right; Direction.Down; Direction.Left ]
    |> Seq.map Point.op_Implicit
    |> Seq.map (fun p -> p.X + x, p.Y + y)
    |> List.ofSeq

let search map x0 y0 p2 =
    let w = Array2D.length1 map
    let h = Array2D.length2 map

    let rec searchInn =
        function
        | [] -> 0
        | (x, y, _) :: tail when x < 0 || y < 0 || x >= w && y >= h -> searchInn tail
        | (x, y, v) :: tail when List.contains (x, y) v -> searchInn tail
        | (x, y, visits) :: tail ->
            match Array2D.get map x y with
            | Wall -> searchInn tail
            | _ when y = h - 1 ->  
                let curr = List.length visits
                max curr (searchInn tail)
            | _ ->
                let newVisits = visits @ [x, y]
                let neighbors = neig map x y p2
                let q = tail @ (List.map (function (x, y) -> (x, y, newVisits)) neighbors)
                    
                searchInn q

    searchInn [ x0, y0, [] ]



[<DateInfo(2023, 23, AdventParts.PartTwo)>]
type Day23() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        let map = this.Input.SquareMap() |> Array2D.map mapCell

        let startX =
            map
            |> Array2D.toSeq
            |> Seq.indexed
            |> Seq.where (snd >> (<>) Wall)
            |> Seq.head
            |> fst

        this.WriteLine $"Starting at {startX}"

        search map startX 0 false

    override this.SolvePartTwo() =
        let map = this.Input.SquareMap() |> Array2D.map mapCell

        let startX =
            map
            |> Array2D.toSeq
            |> Seq.indexed
            |> Seq.where (snd >> (<>) Wall)
            |> Seq.head
            |> fst

        this.WriteLine $"Starting at {startX}"

        search map startX 0 true
