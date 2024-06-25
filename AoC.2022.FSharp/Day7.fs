module AoC._2022.FSharp.Day7

open AoC.FSharp
open AoC.Library.Runner

[<Literal>]
let example =
    """
$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k
"""

type private Object =
    | File of int
    | Directory of Map<string, Object>

type private State =
    { CurrentDirectory: string list
      Root: Object }

type private SizedObject =
    | SizedFile of int
    | SizedDirectory of SizedObject list * string * int 
    

let rec private addFile path name object dir =
    match dir, path with
    | File _, _ -> failwith "Cant add object to file"
    | Directory(objects), [] -> Directory(Map.add name object objects)
    | Directory(objects), subname :: tail ->
        let subdir' =
            match Map.tryFind subname objects with
            | Some subdir -> addFile tail name object subdir
            | None -> addFile tail name object (Directory(Map.empty))

        Directory(Map.add subname subdir' objects)

let private cd dir state =
    match dir with
    | "/" -> { state with CurrentDirectory = [] }
    | ".." ->
        { state with
            CurrentDirectory = state.CurrentDirectory[..^1] }
    | _ ->
        { state with
            CurrentDirectory = state.CurrentDirectory @ [ dir ] }

let private apply state command =
    let parts = String.smartSplit " " command

    match parts with
    | [ "$"; "cd"; dir ] -> cd dir state
    | [ "$"; "ls" ] -> state
    | [ "dir"; name ] ->
        { state with
            Root = addFile state.CurrentDirectory name (Directory(Map.empty)) state.Root }
    | [ size; name ] ->
        { state with
            Root = addFile state.CurrentDirectory name (File(int size)) state.Root }
    | _ -> failwithf $"Unknown command: %s{command}"


let rec private printObject indent name obj =
    match obj with
    | File(size) -> printfn $"%s{indent}%s{name} %d{size}"
    | Directory(objects) ->
        printfn $"%s{indent}%s{name}"

        objects
        |> Map.iter (fun childname obj -> printObject (indent + "  ") childname obj)
        
        
let private size =
    function
    | SizedFile s -> s
    | SizedDirectory (_, _, s) -> s

let rec private toSized name dir = 
    match dir with
    | File size -> SizedFile size
    | Directory(children) -> 
        let children' = 
            children
            |> Map.toList
            |> List.map (Helpers.t2 toSized)
            
        let size = children' |> List.sumBy size

        SizedDirectory(children', name, size)

let rec private printSized indent name obj =
    match obj with
    | SizedFile(size) -> printfn $"%s{indent}%s{name} - %d{size}"
    | SizedDirectory(children, name, size) ->
        printfn $"%s{indent}%s{name} - %d{size}"
    
        let childname =
            function
            | SizedFile _ -> "file"
            | SizedDirectory (_, name, _) -> name
        
        children
        |> List.iter (fun child -> printSized (indent + "  ") (childname child) child)
        
        
let rec private unfold acc =
    function
    | SizedFile _ -> acc @ []
    | SizedDirectory(children, _, _) as dir ->
        children
        |> List.fold unfold acc
        |> (@) [dir]

[<DateInfo(2022, 7, AdventParts.PartOne)>]
[<CustomExample(example, "95437", "24933642")>]
type Day6() =
    inherit AdventSolution()

    override this.SolvePartOne() =
        let state =
            this.Input.Lines
            |> Array.fold
                apply
                { Root = Directory(Map.empty)
                  CurrentDirectory = [] }

        let sized = toSized "/" state.Root
        let dirs = unfold [] sized
        
        printObject "" "/" state.Root        
        // printSized "" "/" sized
        
        dirs
        |> List.map size 
        |> Seq.where (fun s -> s < 100000)
        |> Seq.sum
        :> obj

    override this.SolvePartTwo() = 
        let state =
            this.Input.Lines
            |> Array.fold
                apply
                { Root = Directory(Map.empty)
                  CurrentDirectory = [] }

        let sized = toSized "/" state.Root
        let dirs = unfold [] sized
        let sizes = List.map size dirs
        
        let totalSpace = 70000000
        let required = 30000000
        let used = size sized
        
        sizes
        |> List.sort
        |> List.find (fun s -> totalSpace - used + s >= required)
        :> obj
        
