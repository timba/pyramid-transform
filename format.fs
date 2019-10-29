module Pyramid.Format

let parse (str:string) =
    str 
    |> Seq.toList 
    |> List.rev
    |> List.map (fun x -> 
        match x with 
        | n when n = '0' || n = '1' -> string n |> int
        | _ -> failwith "Only 0 and 1 allowed")

let convert (lst:int list) =
    lst 
    |> List.rev
    |> List.map string
    |> List.fold (+) ("")
