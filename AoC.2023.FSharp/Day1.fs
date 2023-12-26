module AoC._2023.FSharp.Day1

open System
open AoC.Library.Runner

let numbers =
    [ "zero"
      "one"
      "two"
      "three"
      "four"
      "five"
      "six"
      "seven"
      "eight"
      "nine" ]

let parseFirst s =
    let digits = Seq.filter Char.IsDigit s
    let first = Seq.head digits
    let last = Seq.last digits

    int (first - '0') * 10 + int (last - '0')

let prepareSecond s =
    numbers
    |> List.indexed
    |> List.fold (fun (state: string) (i, num) -> state.Replace(num, $"{num}{i}{num}")) s


[<DateInfo(2023, 1, AdventParts.All)>]
type Day1() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        upcast (this.Input.Lines |> Seq.map parseFirst |> Seq.sum)

    override this.SolvePartTwo() =
        upcast (this.Input.Lines |> Seq.map (prepareSecond >> parseFirst) |> Seq.sum)
