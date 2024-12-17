module AoC._2024.FSharp.Day16

open System
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections


let rotateLeft =
    function
    | Direction.Up -> Direction.Left
    | Direction.Right -> Direction.Up
    | Direction.Down -> Direction.Right
    | _ -> Direction.Down

let rotateRight =
    function
    | Direction.Up -> Direction.Right
    | Direction.Right -> Direction.Down
    | Direction.Down -> Direction.Left
    | _ -> Direction.Up


type PathInfo =
    { Prevs: PointBase<int> option list
      Cost: int64 }

[<DateInfo(2024, 16, AdventParts.PartTwo)>]
type Day16() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let map = this.Input.SquareMap((=) '#')
        let chars = this.Input.SquareMap()
        let start = Array2D.filterArray chars ((=) 'S') |> Seq.head |> Point.ofTuple
        let finish = Array2D.filterArray chars ((=) 'E') |> Seq.head |> Point.ofTuple


        let path = Array2D.map (fun _ -> { Prevs = []; Cost = 99999999999L }) map

        let addPathPoint pos prev cost =
            let curr = Array2D.getp path pos

            if cost = curr.Cost then
                Array2D.setp path pos { curr with Prevs = prev :: curr.Prevs }
            else
                Array2D.setp path pos { Cost = cost; Prevs = [ prev ] }

        let getCost pos =
            (Array2D.getp path pos).Cost

        let rec find =
            function
            | [] -> getCost finish
            | (pos, _, _, _) :: tail when Array2D.isInsideP path pos |> not -> find tail
            | (pos, _, _, _) :: tail when Array2D.getp map pos -> find tail
            | (pos, _, prev, price) :: tail when price = getCost pos && List.contains prev (Array2D.getp path pos).Prevs ->
                find tail
            | (pos, currDir, prev, price) :: tail when price <= getCost pos ->
                addPathPoint pos prev price

                let left = rotateLeft currDir
                let right = rotateRight currDir

                tail
                @ [ (pos + Point.ofDir currDir, currDir, Some pos, price + 1L)
                    (pos + Point.ofDir left, left, Some pos, price + 1001L)
                    (pos + Point.ofDir right, right, Some pos, price + 1001L) ]
                |> find
            | _ :: tail -> find tail

        let c = find [ (start, Direction.Right, None, 0) ]

        let rec findAllPaths res extraRes =
            function
            | [] -> extraRes
            | head :: tail when List.contains (Tuple.ofPoint head) res -> findAllPaths res extraRes tail
            | head :: tail ->
                let prevs = List.choose id (Array2D.getp path head).Prevs

                if List.length prevs > 1 then
                    findAllPaths (Tuple.ofPoint head :: res) (Tuple.ofPoint head :: extraRes) (tail @ prevs)
                else
                    findAllPaths (Tuple.ofPoint head :: res) extraRes (tail @ prevs)

        let path_ = findAllPaths [] [] [ finish ]

        for y in 0 .. (this.Input.Height - 1) do
            for x in 0 .. (this.Input.Width - 1) do
                if List.contains (x, y) path_ then printf "X"
                elif map[x, y] then printf "#"
                else printf $"%d{(Array2D.get path x y).Prevs |> List.length}"

            printfn ""

        if isFirst then c else List.length path_

    [<CustomRun("""#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################""")>]
    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
