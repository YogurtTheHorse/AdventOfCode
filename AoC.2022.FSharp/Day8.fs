module AoC._2022.FSharp.Day8

open AoC.Library.Runner

[<DateInfo(2022, 8, AdventParts.PartOne)>]
type Solution() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        let width = this.Input.Width
        let height = this.Input.Height
        let input = Array2D.init width height (fun x y -> int <| string this.Input[x, y])

        let upMin =
            Seq.fold
                (fun lines y ->
                    lines
                    @ [ [ for x in 0 .. width - 1 -> if y > 0 then max input[x, y - 1] lines[y - 1].[x] else -1 ] ])
                []
                [ 0 .. height - 1 ]

        let downMin =
            Seq.fold
                (fun lines y ->
                    lines
                    @ [ [ for x in 0 .. width - 1 ->
                              if y > 0 then
                                  max input[x, height - y] lines[y - 1].[x]
                              else
                                  -1 ] ])
                []
                [ 0 .. height - 1 ]
            |> List.rev

        let leftMin =
            Seq.fold
                (fun lines x ->
                    lines
                    @ [ [ for y in 0 .. height - 1 -> if x > 0 then max input[x - 1, y] lines[x - 1].[y] else -1 ] ])
                []
                [ 0 .. width - 1 ]

        let rightMin =
            Seq.fold
                (fun lines x ->
                    lines
                    @ [ [ for y in 0 .. height - 1 ->
                              if x > 0 then
                                  max input[width - x, y] lines[x - 1].[y]
                              else
                                  -1 ] ])
                []
                [ 0 .. width - 1 ]
            |> List.rev


        seq {
            for y in 0 .. height - 1 do
                for x in 0 .. width - 1 do
                    let m =
                        List.min
                            [ upMin[y][x]
                              downMin[y][x]
                              leftMin[x][y]
                              rightMin[x][y] ]

                    let res = input[x, y] > m

                    yield res
                printfn ""
        }
        |> Seq.where id
        |> Seq.length
        :> obj

    override this.SolvePartTwo() = 0
