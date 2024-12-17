module AoC._2024.FSharp.Day17

open System
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections

type Reg =
    { A: int64
      B: int64
      C: int64
      Output: int list }

let EmptyReg = { A = 0; B = 0; C = 0; Output = [] }

let parseCommands (v: string) =
    v.SmartSplit(",") |> List.ofArray |> List.map int

let rec run maxOutput commands cursor ctx =
    if (cursor >= List.length commands) || (ctx.Output |> List.length >= maxOutput) then
        ctx
    else
        let opc = commands[cursor]
        let op = commands[cursor + 1]

        let comobs =
            match op with
            | 0
            | 1
            | 2
            | 3 -> string op
            | 4 -> "A"
            | 5 -> "B"
            | 6 -> "C"
            | _ -> "-"

        let combo =
            match op with
            | 0
            | 1
            | 2
            | 3 -> int64 op
            | 4 -> ctx.A
            | 5 -> ctx.B
            | 6 -> ctx.C
            | _ -> 0

        match opc with
        | 0 ->
            // printfn $"A = A / 2^%s{comobs}"

            run
                maxOutput
                commands
                (cursor + 2)
                { ctx with
                    A = int (ctx.A / (pown 2L (int combo))) }
        | 1 ->
            // printfn $"B = B xor {op}"
            run maxOutput commands (cursor + 2) { ctx with B = ctx.B ^^^ op }
        | 2 ->
            // printfn $"B = {comobs} %% 8"
            run maxOutput commands (cursor + 2) { ctx with B = combo % 8L }
        | 3 when ctx.A <> 0 ->
            // printfn $"jmz A {comobs}"
            run maxOutput commands op ctx
        | 3 ->
            // printfn $"jmz A {comobs}"
            run maxOutput commands (cursor + 2) ctx
        | 4 ->
            // printfn "B = B xor C"
            run maxOutput commands (cursor + 2) { ctx with B = ctx.B ^^^ ctx.C }
        | 5 ->
            // printfn $"out {comobs} %% 8"
            let v = int (combo % 8L)

            run maxOutput commands (cursor + 2) { ctx with Output = ctx.Output @ [ v ] }
        | 6 ->
            // printfn $"B = A / 2^%s{comobs}"

            run
                maxOutput
                commands
                (cursor + 2)
                { ctx with
                    B = (ctx.A / (pown 2L (int combo))) }
        | 7 ->
            // printfn $"C = A / 2^%s{comobs}"

            // Helpers.printan ctx.Output
            run
                maxOutput
                commands
                (cursor + 2)
                { ctx with
                    C = (ctx.A / (pown 2L (int combo))) }
        | _ -> run maxOutput commands (cursor + 2) ctx

[<DateInfo(2024, 17, AdventParts.PartTwo)>]
type Day17() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let readRegister i =
            int (this.Input.Lines[i].SmartSplit(": ")[1])

        let reg =
            { A = readRegister 0
              B = readRegister 1
              C = readRegister 2
              Output = [] }

        let commands = parseCommands (this.Input.Lines[3].SmartSplit(": ")[1])

        if isFirst then
            let res = run 100 commands 0 reg

            String.Join(",", List.map string res.Output) :> obj
        else
            let rec solve leftNums (state: int64) =
                match leftNums with
                | [] -> true, state
                | num :: tail ->
                    Seq.init 8 (int64 >> ((+) (state <<< 3)))
                    |> Seq.map (fun a -> a, run 1 commands 0 { EmptyReg with A = a })
                    |> Seq.where (fun (_, ctx) -> List.length ctx.Output > 0 && ctx.Output[0] = num)
                    |> Seq.map (fst >> (solve tail))
                    |> Seq.where fst
                    |> Seq.map snd
                    |> Seq.tryHead
                    |> function
                        | Some a -> true, a
                        | _ -> false, 0

            solve (List.rev commands) 0 |> snd :> obj




    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
