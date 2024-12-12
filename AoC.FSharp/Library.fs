namespace AoC.FSharp

open System

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
     
    let printa a =
        printf $"%A{a}\n"


module Array2D =
    let get2 pos map = pos |> Helpers.t2 (Array2D.get map)
    let get2_ map = Helpers.t2 (Array2D.get map)
    let set2 map = Helpers.t2 (Array2D.set map)
    
    let isInside map x y =
        x >= 0 && y >= 0 && x < Array2D.length1 map && y < Array2D.length2 map
    
    let isInside2 map = Helpers.t2 (isInside map) 

    let filterArray map pred =
        seq {
            for x in 0 .. (Array2D.length1 map - 1) do
                for y in 0 .. (Array2D.length2 map - 1) ->
                    if pred map[x, y] then Some(x, y) else None
        }
        |> Seq.choose id
        
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

    let count seq elem =
        seq |> Seq.where ((=) elem) |> Seq.length


    let unzip sequence =
        Seq.foldBack (fun (a, b) (accA, accB) -> a :: accA, b :: accB) sequence ([], [])

    let rec zip (a, b) =
        match (a, b) with
        | ha :: ta, hb :: tb -> (ha, hb) :: zip (ta, tb)
        | _, _ -> []

module String =
    let smartSplit (split: string) (s: string) =
        let a =
            s.Split(split, StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
            |> Seq.map (_.Trim())
            |> List.ofSeq

        a

    let isEmpty s = System.String.IsNullOrEmpty(s)

    let containsChar (c: char) (s: string) = s.Contains(c)

    let containsChar2 (s: string) (c: char) = s.Contains(c)

    let halves (s: string) = s[.. s.Length / 2], s[s.Length / 2 ..]
