module Pyramid.Reduce

open Pyramid.Pyramid

let rec reduceInt stateFn pyramid =
    match pyramid with
    | Cell n -> n
    | _      -> 
        let isCompacted, compacted = compactCheck pyramid
        if isCompacted then 
            stateFn compacted
            reduceInt stateFn compacted
        else 
            let checkSumed = applyCheckSum pyramid
            stateFn checkSumed
            reduceInt stateFn checkSumed

let reduce stateFn pyramid =
    stateFn pyramid
    reduceInt stateFn pyramid