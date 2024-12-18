module AoC._2024.FSharp.Day18

open System
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections

let offsets = [ (0, 1); (1, 0); (0, -1); (-1, 0) ] |> List.map Point.ofTuple

type Queue = (PointBase<int> * int) Queue

[<DateInfo(2024, 18, AdventParts.PartOne)>]
type Day18() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let size = if this.Input.IsExample then 7 else 71

        let broken =
            this.Input.Lines
            |> List.ofArray
            |> List.map (_.SmartSplit(","))
            |> List.map (Array.map int >> Tuple.ofArray2)

        let map = Array2D.init size size (fun _ _ -> false)

        let rec bfs visited (q: Queue) =
            match q.TryHead with
            | None ->
                for y in 0 .. size - 1 do
                    for x in 0 .. size - 1 do
                        if List.contains (PointBase<int>(x, y)) visited then
                            printf "O"
                        elif Array2D.get map x y then
                            printf "#"
                        else
                            printf "."

                    printfn ""

                -1
            | Some(n, _) when List.contains n visited -> bfs visited q.Tail
            | Some(n, len) when n.X = size - 1 && n.Y = size - 1 -> len
            | Some(n, len) ->
                let nq =
                    offsets
                    |> Seq.map ((+) n)
                    |> Seq.where (Array2D.isInsideP map)
                    |> Seq.where ((Array2D.getp map) >> not)
                    |> Seq.map (fun n -> (n, len + 1))
                    |> Seq.fold (fun (acc: Queue) -> acc.Conj) q

                bfs (n :: visited) nq


        let start = (Queue.ofList [ (PointBase<int>(0, 0), 0) ])

        let test i =
            Array2D.set2 map broken[i] true

            printfn $"{i}"

            bfs [] start

        let breaks =
            Seq.init (List.length broken) id
            |> Seq.map (fun i -> i, test i)
            |> Seq.where (fun (_, tst) -> tst < 0)
            |> Seq.map fst
            |> Seq.head

        printfn $"{breaks}"
        printfn $"{bfs [] start}"
        broken[breaks]





    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
