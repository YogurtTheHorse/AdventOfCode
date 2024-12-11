module AoC._2024.FSharp.Day11

open System.Collections.Generic
open AoC.Library.Runner
open AoC.Library.Utils

let i2024 = int64 2024
let i1 = int64 1


let rec calc (cache: Dictionary<int64 * int, int64>) times stone =
    if times <= 0 then
       i1
    else
        match cache.TryGetValue((stone, times)) with
        | true, v -> v
        | false, _ ->
            let s =
                if stone = 0 then
                    calc cache (times - 1) 1
                else
                    let sv = string stone

                    if sv.Length % 2 = 0 then
                        let a = int64 sv[0 .. (sv.Length / 2 - 1)]
                        let b = int64 sv[(sv.Length / 2) .. sv.Length]

                        (calc cache (times - 1) a) + (calc cache (times - 1) b)
                    else
                        calc cache (times - 1) (stone * i2024)

            cache.Add((stone, times), s)

            s



[<DateInfo(2024, 11, AdventParts.All)>]
[<CustomExample("125 17", "55312", "65601038650482")>]
type Day11() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let stones = this.Input.Lines[0].SmartSplit(" ") |> List.ofSeq |> List.map (int64)
        let times = if isFirst then 25 else 75
        let cache = Dictionary<int64 * int, int64>()

        stones |> List.map (calc cache times) |> Seq.sum

    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
