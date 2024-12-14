module AoC._2024.FSharp.Day14

open System.Collections.Generic
open AoC.FSharp
open AoC.Library.Runner
open AoC.Library.Utils

open Plotly.NET


type Point = PointBase<int>
type Robot = { Pos: Point; Velocity: Point }

let parseRobot (s: string) =
    let ps, vs = s.SmartSplit(" ") |> Tuple.ofArray2

    let parsePoint (p: string) =
        (p.SmartSplit("=")[1]).SmartSplit(",")
        |> Array.map int
        |> Tuple.ofArray2
        |> Point.ofTuple

    { Pos = parsePoint ps
      Velocity = parsePoint vs }


[<DateInfo(2024, 14, AdventParts.PartTwo)>]
type Day13() =
    inherit AdventSolution()

    member this.Solve isFirst =
        let robots = this.Input.Lines |> Array.map parseRobot |> List.ofArray
        let w, h = if this.Input.IsExample then (11, 7) else (101, 103)

        let moveRobot time robot =
            { robot with
                Pos = (robot.Pos + robot.Velocity * time).Loop(w, h) }
            
            
            
        let freqMap = Array2D.init w h (fun _ _ -> 0)
            
        let printRobots robots =
            let poss = robots |> List.map (_.Pos >> Tuple.ofPoint)
            for y in 0..h - 1 do
                for x in 0..w - 1 do
                    let c = List.count poss (x, y) 
                    if c > 0 then
                        printf $"%d{c}"
                    else printf "."
                printfn ""
                
            printfn "\n"
            
        let fillFreq poses =
            for pos in poses do
                let v = Array2D.get2_ freqMap pos 
                Array2D.set2 freqMap pos (v + 1) 

        let quadr (v: Point) =
            let isLeft = v.X < w / 2
            let isTop = v.Y < h / 2
            let isInTheMiddle = v.X * 2 + 1 = w || v.Y * 2 + 1 = h

            if isInTheMiddle then
                None
            else
                match isLeft, isTop with
                | true, true -> 0
                | false, true -> 1
                | true, false -> 2
                | false, false -> 3
                |> Some

        if isFirst then
            robots
            |> List.map (moveRobot 100)
            |> List.choose (_.Pos >> quadr)
            |> Helpers.silent (printf "%A")
            |> List.countBy id
            |> List.map snd
            |> Helpers.silent (printf "%A")
            |> List.fold (*) 1
        else
            for i in 1..1000000 do
                robots
                |> List.map ((moveRobot i) >> _.Pos >> Tuple.ofPoint)
                |> fillFreq 
                
            for y in 0..h - 1 do
                for x in 0..w - 1 do
                    printf $"%d{Array2D.get freqMap x y},"
                printfn ""
                
            let newFreq = Seq.init h (fun y -> Seq.init w (fun x -> Array2D.get freqMap x y))
            
            Chart.Heatmap(zData=newFreq)
            |> Chart.withSize(700.,500.)
            |> Chart.withMarginSize(Left=200.)
            |> Chart.show
            
            0


    override this.SolvePartOne() = this.Solve true

    override this.SolvePartTwo() = this.Solve false
