module AoC._2024.FSharp.Day3

open System.Text.RegularExpressions
open AoC.FSharp
open AoC.Library.Runner


let dosreg = Regex(@"(do|don\'t)\(\)", RegexOptions.Compiled)
let mulreg = Regex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled)


let rec filter muls dos state =
    let nextDos = List.tryHead dos |> Option.defaultValue (99999999, false)

    match muls with
    | mul :: mulsTail when fst mul < fst nextDos ->
        if state then
            snd mul :: (filter mulsTail dos state)
        else
            filter mulsTail dos state
    | mul :: _ when fst mul >= fst nextDos -> filter muls (List.skip 1 dos) (snd nextDos)
    | [] -> []
    | _ -> failwith "todo"



[<DateInfo(2024, 3, AdventParts.PartTwo)>]
type Day3() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        mulreg.Matches(this.Input.Raw)
        |> List.ofSeq
        |> List.map (fun m -> int m.Groups[1].Value * int m.Groups[2].Value)
        |> List.sum
        :> obj


    [<CustomRun("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))")>]
    override this.SolvePartTwo() =
        let muls =
            mulreg.Matches(this.Input.Raw)
            |> List.ofSeq
            |> List.map (fun m -> m.Index, int m.Groups[1].Value * int m.Groups[2].Value)

        let dos =
            dosreg.Matches(this.Input.Raw)
            |> List.ofSeq
            |> List.map (fun m -> m.Index, m.Groups[1].Value = "do")

        filter muls dos true |> List.sum :> obj
