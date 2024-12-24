module AoC._2024.FSharp.Day20

open System
open System.Collections.Generic
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils
open FSharpx.Collections

let offsets = [ (0, 1); (1, 0); (0, -1); (-1, 0) ] |> List.map Point.ofTuple

type Queue = (PointBase<int> * int) Queue


[<DateInfo(2024, 20, AdventParts.PartOne)>]
type Day20() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let map = this.Input.SquareMap((=) '#')
        let chars = this.Input.SquareMap()
        let start = Array2D.filterArray chars ((=) 'S') |> Seq.head |> Point.ofTuple
        let finish = Array2D.filterArray chars ((=) 'E') |> Seq.head |> Point.ofTuple
        
        let rec findWay ignoreWall maxLen visited (q: Queue) =
            match q.TryHead with
            | None -> None
            | Some (_, currentLen) when currentLen >= maxLen -> None
            | Some (f, currentLen) when f = finish -> Some currentLen
            | Some (f, _) when List.contains f visited ->
                findWay ignoreWall maxLen visited q.Tail
            | Some (head, currentLen) ->
                let newQ =
                    offsets
                    |> Seq.map ((+) head)
                    |> Seq.where (Array2D.isInsideP map)
                    |> Seq.where (fun n ->
                        n = ignoreWall || not (Array2D.getp map n)
                    )
                    |> Seq.map (fun n -> n, currentLen + 1)
                    |> Seq.fold (fun (acc: Queue) -> acc.Conj) q.Tail
                findWay ignoreWall maxLen (head :: visited) newQ
                
        let startQueue = Queue.ofList [(start, 0)]
        let originalLen =
            findWay PointBase<int>.Zero 99999999 [] startQueue
            |> Option.defaultValue 99999999 
        
        printfn $"Original len is {originalLen}"
        
        Array2D.filterArray map id
        |> Seq.choose (fun w -> findWay (Point.ofTuple w) (originalLen - 100) [] startQueue)
        |> Seq.map (Helpers.silent Helpers.printan)
        |> Seq.length
                

    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
