namespace AoC.FSharp

open System
open AoC.Library.Utils


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


    let ofPoint (p: PointBase<'a>) = p.X, p.Y

module Point =
    let ofTuple (x, y) = PointBase(x, y)

    let ofArray a =
        a |> Tuple.ofArray2 |> PointBase

    let ofDir (d: Direction) =
        PointBase.op_Implicit d

    let min p1 p2 =
        PointBase.Min(p1, p2)

    let max p1 p2 =
        PointBase.Max(p1, p2)

    let bounds p1 p2 =
        PointBase.Bounds(p1, p2)

    let length (p1: PointBase<'a>) = p1.Length

    let isInBounds w (h: 'a) (p: PointBase<'a>) = p.InBounds(w, h)
        
    let isInBounds2 b1 (b2: PointBase<'a>) (p: PointBase<'a>) = p.InBounds(b1, b2)

module Helpers =
    let t2 foo (x, y) = foo x y
    let t3 foo (x, y, z) = foo x y z

    let silent f x =
        f x
        x

    let printa a = printf $"%A{a}"

    let printan a = printfn $"%A{a}"


module Array2D =
    let getp map (p: PointBase<int>) =
        Array2D.get map p.X p.Y

    let setp map (p: PointBase<int>) v =
        Array2D.set map p.X p.Y v

    let get2 map =
        Helpers.t2 (Array2D.get map)

    let set2 map =
        Helpers.t2 (Array2D.set map)

    let isInside map x y =
        x >= 0 && y >= 0 && x < Array2D.length1 map && y < Array2D.length2 map

    let isInside2 map =
        Helpers.t2 (isInside map)

    let isInsideP map (p: PointBase<int>) =
        isInside map p.X p.Y

    let filterArray map pred =
        seq {
            for x in 0 .. (Array2D.length1 map - 1) do
                for y in 0 .. (Array2D.length2 map - 1) -> if pred map[x, y] then Some(x, y) else None
        }
        |> Seq.choose id

    let toSeq arr =
        seq {
            for y in 0 .. Array2D.length2 arr - 1 do
                for x in 0 .. Array2D.length1 arr - 1 do
                    yield arr.[x, y]
        }

module List =
    let removeAt i list =
        list
        |> List.mapi (fun i e -> i, e)
        |> List.where (fst >> ((<>) i))
        |> List.map snd

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

    let startsWith (s: string) (sub: string) = s.StartsWith(sub)

    let startsWithFrom (s: string) f (sub: string) = s.Substring(f).StartsWith(sub)

    let length (s: string) = s.Length
    
    let longLength (s: string) = int64 s.Length

    let isEmpty s =
        System.String.IsNullOrEmpty(s)

    let containsChar (c: char) (s: string) = s.Contains(c)

    let containsChar2 (s: string) (c: char) = s.Contains(c)

    let halves (s: string) =
        s[.. s.Length / 2], s[s.Length / 2 ..]
