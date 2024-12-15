module AoC._2024.FSharp.Day6

open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

let offset =
    function
    | Direction.Up -> (0, -1)
    | Direction.Down -> (0, 1)
    | Direction.Right -> (1, 0)
    | Direction.Left -> (-1, 0)
    | _ -> failwith "Not supported direction"

let nextDir =
    function
    | Direction.Up -> Direction.Right
    | Direction.Right -> Direction.Down
    | Direction.Down -> Direction.Left
    | Direction.Left -> Direction.Up
    | _ -> failwith "Not supported direction"

let add a b =
    let ax, ay = a
    let bx, by = b

    (ax + bx, ay + by)

let filterArray pred map =
    seq {
        for x in 0 .. (Array2D.length1 map - 1) do
            for y in 0 .. (Array2D.length2 map - 1) -> if pred map[x, y] then Some(x, y) else None
    }
    |> Seq.choose id

let filterMap char map =
    filterArray ((=) char) map


let rec step map pos direction visited =
    let w = Array2D.length1 map
    let h = Array2D.length2 map
    let nPos = add pos (offset direction)
    let nx, ny = nPos

    let addVisited () =
        let curr = Array2D.get2 visited pos
        Array2D.set2 visited pos (direction :: curr)

        visited

    if nx < 0 || nx >= w || ny < 0 || ny >= h then
        visited, false
    elif List.contains direction (Array2D.get2 visited pos) then
        visited, true
    elif Array2D.get2 map nPos = '#' then
        step map pos (nextDir direction) (addVisited ())
    else
        step map nPos direction (addVisited ())

[<DateInfo(2024, 6, AdventParts.All)>]
type Day6() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        let map = this.Input.SquareMap()

        let guardPos = filterMap '^' map |> Seq.head
        let visited, _ = step map guardPos Direction.Up (Array2D.map (fun _ -> []) map)

        filterArray ((<>) []) visited |> Seq.distinct |> Seq.length |> (+) 1 :> obj

    override this.SolvePartTwo() =
        let map = this.Input.SquareMap()
        let guardPos = filterMap '^' map |> Seq.head

        filterMap '.' map
        |> Seq.where (fun (wx, wy) ->
            let newMap = map |> Array2D.copy
            Array2D.set newMap wx wy '#'

            let _, isLoop = step newMap guardPos Direction.Up (Array2D.map (fun _ -> []) map)

            isLoop)
        |> Seq.length
        :> obj
