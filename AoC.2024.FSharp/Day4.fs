module AoC._2024.FSharp.Day4

open AoC.Library.Runner

let rec extract (map: char array2d) leftCnt dir (p: int * int) =
    let x, y = p

    if x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1) then
        ""
    else if leftCnt > 1 then
        string map[x, y] + (extract map (leftCnt - 1) dir (x + fst dir, y + snd dir))
    else
        string map[x, y]


[<DateInfo(2024, 4, AdventParts.All)>]
[<CustomExample("""MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX""",
                "18",
                "9")>]
type Day4() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        let map = this.Input.SquareMap()

        seq {
            for col in 0 .. this.Input.Height - 1 do
                for row in 0 .. this.Input.Width - 1 do
                    for dx in -1 .. 1 do
                        for dy in -1 .. 1 -> (row, col), (dx, dy)
        }
        |> Seq.where (snd >> (<>) (0, 0))
        |> Seq.where (fun (p, d) -> (extract map 4 d p) = "XMAS")
        |> Seq.length
        :> obj


    override this.SolvePartTwo() =
        let map = this.Input.SquareMap()

        seq {
            for col in 0 .. this.Input.Height - 1 do
                for row in 0 .. this.Input.Width - 1 -> (row, col)
        }
        |> Seq.where (fun (x, y) ->
            let f = extract map 3 (1, 1) (x, y)
            let s = extract map 3 (-1, 1) (x + 2, y)

            (f = "MAS" || f = "SAM") && (s = "MAS" || s = "SAM"))
        |> Seq.length
        :> obj
