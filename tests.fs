module Pyramid.Tests

open Xunit

open Pyramid.Pyramid
open Pyramid.Reduce
open Pyramid.Format

[<Fact>]
let ``create hould construct 4^0 pyramid`` () =
    let pyramid = create [1]
    Assert.True((pyramid = Cell 1))

[<Fact>]
let ``create should construct 4^1 pyramid`` () =
    let pyramid = create [1;0;1;1]
    Assert.True((pyramid = Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 1)))

[<Fact>]
let ``create should construct 4^2 pyramid`` () =
    let pyramid = create [1;0;1;1;0;0;1;0;0;0;0;0;1;1;1;1]
    Assert.True((pyramid = Pyramids(
                                Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 1),
                                Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0),
                                Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 0),
                                Pyramids(Cell 0 ,Cell 1, Cell 1, Cell 1))))

[<Fact>]
let ``create should fail when input length is not power of 4`` () =
    let act = fun () -> create [1;0;1;1;0;0] |> ignore
    Assert.Throws(act) |> ignore

[<Fact>]
let ``compactCheck should compact 4^1 pyramid of 1s to 1`` () =
    let pyramid = Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 1)
    let isCompacted, compacted = compactCheck pyramid
    Assert.True((compacted = Cell 1))
    Assert.True(isCompacted)

[<Fact>]
let ``compactCheck should compact 4^2 pyramid of 1s and 0s to pyramid 4^1`` () =
    let pyramid = Pyramids(
                        Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 1),
                        Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0),
                        Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 1),
                        Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0))

    let isCompacted, compacted = compactCheck pyramid
    Assert.True((compacted = Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 0)))
    Assert.True(isCompacted)

[<Fact>]
let ``compactCheck should not compact uncompactible pyramid`` () =
    let pyramid = Pyramids(
                        Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 1),
                        Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0),
                        Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 1),
                        Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0))

    let isCompacted, compacted = compactCheck pyramid
    Assert.True((compacted = pyramid))
    Assert.False(isCompacted)
    
[<Fact>]
let ``applyCheckSum should check sum 4^1`` () =
    let pyramid = Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 1)
    let processed = applyCheckSum pyramid
    Assert.True((processed = Pyramids(Cell 0 ,Cell 1, Cell 1, Cell 1)))

[<Fact>]
let ``applyCheckSum should check sum 4^2`` () =
    let pyramid  = Pyramids(
                         Pyramids(Cell 0 ,Cell 0, Cell 1, Cell 0),
                         Pyramids(Cell 1 ,Cell 1, Cell 0, Cell 1),
                         Pyramids(Cell 1 ,Cell 0, Cell 0, Cell 1),
                         Pyramids(Cell 0 ,Cell 1, Cell 0, Cell 1))

    let expected = Pyramids(
                         Pyramids(Cell 0 ,Cell 0, Cell 0, Cell 0),
                         Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 1),
                         Pyramids(Cell 1 ,Cell 0, Cell 1, Cell 0),
                         Pyramids(Cell 1 ,Cell 1, Cell 1, Cell 0))

    let processed = applyCheckSum pyramid
    Assert.True((processed = expected))

[<Fact>]
let ``reduce should reduce pyramid`` () =
    let pyramid = Pyramids
                        (Pyramids (Cell 1, Cell 1, Cell 0, Cell 0),
                         Pyramids (Cell 0, Cell 1, Cell 0, Cell 1),
                         Pyramids (Cell 0, Cell 1, Cell 0, Cell 1),
                         Pyramids (Cell 1, Cell 0, Cell 0, Cell 1))
    let reduced = reduce ignore pyramid
    Assert.Equal(1, reduced)

[<Fact>]
let ``input has to be reverted before pyramid creation`` () =
    let input = "0111"
    let pyramid = input |> parse |> create
    Assert.True((pyramid = Pyramids(Cell 1, Cell 1, Cell 1, Cell 0)))

[<Fact>]
let ``output has to be reverted after pyramid deconstruction`` () =
    let pyramid = Pyramids(Cell 0, Cell 1, Cell 0, Cell 0)
    let output = pyramid |> deconstruct |> convert
    Assert.Equal("0010", output)

[<Fact>]
let ``pyramid create reverse test`` () =
    let input = "1100001000011101001100110110000001101100010111111100111100010111"
    let reverse = input |> parse |> create |> deconstruct |> convert
    Assert.Equal(input, reverse)
