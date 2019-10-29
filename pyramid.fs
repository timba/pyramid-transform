module Pyramid.Pyramid

type Pyramid = 
    | Pyramids of (Pyramid * Pyramid * Pyramid * Pyramid)
    | Cell of int

let pyramidBaseLength len =
    (int (sqrt (float len)) - 1) * 2 + 1

let split num list = 
  List.take num list, List.skip num list

let rec splitLinePyramids input lst1 lst2 lst3 lnNumber lnLen =
    match input with
    | [] -> lst1, lst2, lst3
    | _ ->
        let sideLen = lnNumber * 2 + 1
        let midlLen = lnLen - sideLen * 2

        let left, tmp1   = split sideLen input
        let midl, tmp2   = split midlLen tmp1
        let rigt, remain = split sideLen tmp2

        splitLinePyramids 
            remain
            (lst1 @ left)
            (midl @ lst2) 
            (lst3 @ rigt) 
            (lnNumber + 1)
            (lnLen + 2)

let rec construct lst = 
  match lst with 
  | [a; b; c; d] -> Pyramids (Cell a, Cell b, Cell c, Cell d)
  | [a]          -> Cell a
  | _            -> partition lst

and partition lst =
    let len = List.length lst
    let subPyramidLength = len / 4
    let lst1 = lst |> List.take subPyramidLength

    let remaining = lst |> List.skip subPyramidLength
    let subPyramidBase = pyramidBaseLength subPyramidLength

    let lst2, lst3, lst4 = splitLinePyramids remaining [] [] [] 0 (subPyramidBase + 2)

    Pyramids (construct lst1, construct lst2, construct lst3, construct lst4)


let rec joinLinePyramids lst1 lst2 lst3 lnNumber lnLen =
  match lst1,lst2,lst3 with
  | [],[],[] -> []
  | _        ->
    let sideLen = lnNumber * 2 + 1
    let midlLen = lnLen - sideLen * 2
    let rem2Len = (List.length lst2) - midlLen
    let top1, top2, top3 = 
        lst1 |> List.take sideLen, 
        lst2 |> List.skip rem2Len, 
        lst3 |> List.take sideLen

    let row = top1 @ (top2 @ top3)
    row @ (joinLinePyramids 
            (lst1 |> List.skip sideLen)
            (lst2 |> List.take rem2Len)
            (lst3 |> List.skip sideLen)
            (lnNumber + 1)
            (lnLen + 2))

let rec deconstruct pyramid =
  match pyramid with
  | Cell n                   -> [n]
  | Pyramids(p1, p2, p3, p4) -> (deconstruct p1) @ (unite p2 p3 p4)

and unite p1 p2 p3 =
  let lst1, lst2, lst3 = deconstruct p1, deconstruct p2, deconstruct p3
  let len = List.length lst1
  let pbase = pyramidBaseLength len
  joinLinePyramids lst1 lst2 lst3 0 (pbase + 2)


let rec applyCheckSum pyramid =
    match pyramid with
    | Cell n                                    -> Cell n
    | Pyramids (Cell a, Cell b, Cell c, Cell d) ->
        let d1, c1, b1, a1 = 
            match d, c, b, a with
            | 0,0,0,0 -> 0,0,0,0
            | 0,0,0,1 -> 1,0,0,0
            | 0,0,1,0 -> 0,0,0,1
            | 0,0,1,1 -> 0,0,1,0
            | 0,1,0,0 -> 0,0,0,0
            | 0,1,0,1 -> 0,0,1,0
            | 0,1,1,0 -> 1,0,1,1
            | 0,1,1,1 -> 1,0,1,1
            | 1,0,0,0 -> 0,1,0,0
            | 1,0,0,1 -> 0,1,0,1
            | 1,0,1,0 -> 0,1,1,1
            | 1,0,1,1 -> 1,1,1,1
            | 1,1,0,0 -> 1,1,0,1
            | 1,1,0,1 -> 1,1,1,0
            | 1,1,1,0 -> 0,1,1,1
            | 1,1,1,1 -> 1,1,1,1
            | _ -> failwith "Only 1 and 0 allowed"
        Pyramids (Cell a1, Cell b1, Cell c1, Cell d1)

    | Pyramids (a, b, c, d)                     -> 
        Pyramids (applyCheckSum a, applyCheckSum b, applyCheckSum c, applyCheckSum d)

let rec isReducible pyramid =
  match pyramid with
  | Cell _                                   -> false
  | Pyramids(Cell 1, Cell 1, Cell 1, Cell 1) -> true
  | Pyramids(Cell 0, Cell 0, Cell 0, Cell 0) -> true
  | Pyramids(p1, p2, p3, p4)                 -> 
        isReducible p1 && isReducible p2 && isReducible p3 && isReducible p4

let rec compact pyramid =
    match pyramid with
    | Cell n                                    -> Cell n
    | Pyramids (Cell 0, Cell 0, Cell 0, Cell 0) -> Cell 0
    | Pyramids (Cell 1, Cell 1, Cell 1, Cell 1) -> Cell 1
    | Pyramids (p1, p2, p3, p4)                 -> 
        Pyramids(compact p1, compact p2, compact p3, compact p4)

let rec compactCheck pyramid =
    if pyramid |> isReducible then
        let _, compacted = compactCheck (compact pyramid)
        true, compacted
    else
        false, pyramid

let create lst =
    let len = List.length lst
    let pow = int ((log (float len)) / log 4.0)
    let base4 = (pown 4 pow) = len
    if not base4 then 
      failwithf "Length should be power of 4, your length is %i" len
    construct lst