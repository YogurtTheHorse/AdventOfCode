module AoC._2024.FSharp.Day11

open System.Collections.Generic
open AoC.Library.Runner
open AoC.Library.Utils

let i2024 = int64 2024

let blinkSimple (v: int64) =
    if v = 0 then
        [ int64 1 ]
    else
        let sv = string v

        if sv.Length % 2 = 0 then
            let s = string v
            let a = int64 s[0 .. (s.Length / 2 - 1)]
            let b = int64 s[(s.Length / 2) .. s.Length]

            [ a; b ]
        else
            [ v * i2024 ]

let rec calc (cache: Dictionary<int64 * int, int64>) times stone =
    match cache.TryGetValue((stone, times)) with
    | true, v -> v
    | false, _ ->
        let div = blinkSimple stone

        let s =
            if times > 1 then
                (div |> List.map (calc cache (times - 1)) |> List.sum)
            else
                List.length div

        cache.Add((stone, times), s)

        s



[<DateInfo(2024, 11, AdventParts.All)>]
[<CustomExample("125 17", "55312")>]
type Day11() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let stones = this.Input.Lines[0].SmartSplit(" ") |> List.ofSeq |> List.map (int64)
        let times = if isFirst then 25 else 75
        let cache = Dictionary<int64 * int, int64>()

        stones |> List.map (calc cache times) |> Seq.sum

    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
