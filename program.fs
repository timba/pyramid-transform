open Pyramid.Pyramid
open Pyramid.Format
open Pyramid.Reduce

let print = deconstruct >> convert >> printfn "%s"

[<EntryPoint>]
let main argv =
    if argv |> Array.length <> 1 then
        printfn "Expected one argument"
        1
    else
        try 
            argv.[0] 
            |> parse 
            |> create
            |> reduce print
            |> ignore
            0
        with
        | ex ->
            printfn "Error: %s" ex.Message
            1
