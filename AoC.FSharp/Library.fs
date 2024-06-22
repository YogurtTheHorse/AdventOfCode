namespace AoC.FSharp

module Tuple =
    let ofArray2 =
        function
        | [| a; b |] -> (a, b)
        | _ -> failwith "wrong array"

    let ofArray3 =
        function
        | [| a; b; c |] -> (a, b, c)
        | _ -> failwith "wrong array"

    let ofList2 =
        function
        | [ a; b ] -> (a, b)
        | _ -> failwith "wrong array"

    let ofList3 =
        function
        | [ a; b; c ] -> (a, b, c)
        | _ -> failwith "wrong array"

module Helpers =
    let t2 foo (x, y) = foo x y
    let t3 foo (x, y, z) = foo x y z

    let silent f x =
        f x
        x


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

    let split elem arr =
        Seq.fold
            (fun lists v ->
                match v, lists with
                | v, curr when v = elem -> [] :: curr
                | v, [] -> [ [ v ] ]
                | v, head :: tail -> (head @ [ v ]) :: tail)
            []
            arr
        |> List.rev

module String =
    let smartSplit (split: string) (s: string) =
        s.Split(split) |> Seq.map (_.Trim()) |> List.ofSeq

    let containsChar (c: char) (s: string) = s.Contains(c)

    let containsChar2 (s: string) (c: char) = s.Contains(c)

    let halves (s: string) = s[.. s.Length / 2], s[s.Length / 2 ..]
