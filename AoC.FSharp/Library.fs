namespace AoC.FSharp

module Helpers =
    let t2 foo (x, y) = foo x y


module Array2D =
    let get2 map = Array2D.get map |> Helpers.t2

    let toSeq arr =
        seq {
            for y in 0 .. Array2D.length2 arr - 1 do
                for x in 0 .. Array2D.length1 arr - 1 do
                    yield arr.[x, y]
        }

module List =
    let containsRev lst v = List.contains v lst
