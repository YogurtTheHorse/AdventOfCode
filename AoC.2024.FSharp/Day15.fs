module AoC._2024.FSharp.Day15

open System
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections


type Tile =
    | Wall
    | Empty
    | Box
    | BoxLeft
    | BoxRight
    | Robot

let mapChar =
    function
    | '#' -> Wall
    | 'O' -> Box
    | '[' -> BoxLeft
    | ']' -> BoxRight
    | '@' -> Robot
    | _ -> Empty

let ttoc =
    function
    | Wall -> '#'
    | Box -> 'O'
    | Robot -> '@'
    | BoxLeft -> '['
    | BoxRight -> ']'
    | _ -> '.'

let ctod =
    function
    | '^' -> Direction.Up
    | '>' -> Direction.Right
    | 'v' -> Direction.Down
    | _ -> Direction.Left


let rec canPush map pos (dir: Direction) =
    let ofs = PointBase<int>.op_Implicit dir
    let nextPos = pos + ofs
    let n = Array2D.get2 map (Tuple.ofPoint nextPos)

    match n with
    | Empty -> true
    | Box -> canPush map nextPos dir
    | BoxLeft
    | BoxRight when ofs.Y = 0 -> canPush map nextPos dir
    | BoxLeft -> canPush map nextPos dir && canPush map (nextPos + PointBase.Right) dir
    | BoxRight -> canPush map nextPos dir && canPush map (nextPos + PointBase.Left) dir
    | _ -> false

let rec doPush map pos (dir: Direction) =
    let ofs = PointBase<int>.op_Implicit dir
    let nextPos = pos + ofs
    let curr = Array2D.get map pos.X pos.Y

    let pushOne (f: PointBase<int>) (n: PointBase<int>) =
        Array2D.set map f.X f.Y Empty
        Array2D.set map n.X n.Y curr

    match curr with
    | Empty -> ()
    | _ ->
        doPush map nextPos dir

        pushOne pos nextPos

        if ofs.Y <> 0 then
            match curr with
            | BoxLeft -> doPush map (pos + PointBase.Right) dir
            | BoxRight -> doPush map (pos + PointBase.Left) dir
            | _ -> ()
        else
            ()

let rec tryPush map pos (dir: Direction) =
    if canPush map pos dir then
        doPush map pos dir
        true
    else
        false

let rec act map pos =
    function
    | [] -> ()
    | head :: tail ->
        if tryPush map pos head then
            act map (pos + PointBase<int>.op_Implicit head) tail
        else
            act map pos tail



[<DateInfo(2024, 15, AdventParts.PartTwo)>]
type Day15() =
    inherit AdventSolution()


    member this.Solve isFirst =
        let h = Array.findIndex String.isEmpty this.Input.FullLines

        let inputString =
            if isFirst then
                this.Input.Raw
            else
                this.Input.Raw
                    .Replace("#", "##")
                    .Replace("O", "[]")
                    .Replace(".", "..")
                    .Replace("@", "@.")

        let lines = inputString.SmartSplit("\n")

        let map =
            Array2D.init (if isFirst then this.Input.Width else this.Input.Width * 2) h (fun x y -> lines[y][x])
            |> Array2D.map mapChar

        let actions =
            Seq.init (this.Input.Height - h) (fun i -> this.Input.FullLines[i + h + 1])
            |> Seq.collect (_.ToCharArray())
            |> Seq.map ctod
            |> List.ofSeq


        let startPos = Array2D.filterArray map ((=) Robot) |> Seq.head |> Point.ofTuple

        act map startPos actions


        for i in 0 .. Array2D.length2 map - 1 do
            for j in 0 .. Array2D.length1 map - 1 do
                printf $"%c{ttoc map[j, i]}"

            printfn ""

        printfn "\n"

        Array2D.filterArray map (function
            | Box
            | BoxLeft -> true
            | _ -> false)
        |> Seq.map (fun (x, y) -> x + 100 * y)
        |> Seq.sum


    [<CustomRun("""########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<""")>]
    override this.SolvePartOne() = this.Solve true

    [<CustomRun("""#######
#...#.#
#.....#
#..OO@#
#..O..#
#.....#
#######

<vv<<^^<<^^"""
                + "\n")>]
    override this.SolvePartTwo() = this.Solve false
