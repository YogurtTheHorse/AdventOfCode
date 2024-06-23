module AoC._2022.FSharp.Day6

open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

let find count s =
    let l = List.ofSeq s

    seq { count - 1 .. List.length l }
    |> Seq.map (fun i -> l[i - count + 1 .. i] |> List.distinct)
    // |> Seq.map (Helpers.silent (printfn "%A"))
    |> Seq.findIndex (List.length >> (=) count)
    |> (+) count

[<DateInfo(2022, 6, AdventParts.All)>]
type Day6() =
    inherit AdventSolution()

    [<CustomRun("bvwbjplbgvbhsrlpgdmjqwftvncz", correct = "5")>]
    [<CustomRun("nppdvjthqldpwncqszvftbrmjlhg", correct = "6")>]
    [<CustomRun("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", correct = "10")>]
    [<CustomRun("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", correct = "11")>]
    override this.SolvePartOne() =
        let line = this.Input.Raw.Trim()

        line |> find 4 :> obj

    [<CustomRun("bvwbjplbgvbhsrlpgdmjqwftvncz", correct = "23")>]
    [<CustomRun("nppdvjthqldpwncqszvftbrmjlhg", correct = "23")>]
    [<CustomRun("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", correct = "29")>]
    [<CustomRun("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", correct = "26")>]
    override this.SolvePartTwo() =
        let line = this.Input.Raw.Trim()

        line |> find 14 :> obj
