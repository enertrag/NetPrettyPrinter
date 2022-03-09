// Copyright(c) 2020, ENERTRAG SE
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1.Redistributions of source code must retain the above copyright notice, this
// list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation
// and/or other materials provided with the distribution.
//
// 3. Neither the name of the copyright holder nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

namespace NetPrettyPrinter.FSharp

/// Functions and constants to compose documents that can be pretty printed.
module Doc =
    open System
    open NetPrettyPrinter

    /// Pretty print a document for a prescribed width of 80.
    let render (doc: Doc) = doc.PrettyPrint()

    /// Pretty print a document for the given prescribed width.
    let renderWidth (maxWidth: uint32) (doc: Doc) = doc.PrettyPrint(maxWidth)

    /// The empty document.
    let empty = Doc.Empty

    /// Transform a string into a document.
    let text (content: string) = Doc.Text(content)

    /// Transform an int32 into a document.
    let numInt32 (num: int32) = Doc.Number(num)

    /// Transform a uint32 into a document.
    let numUInt32 (num: uint32) = Doc.Number(num)

    let numInt16 (num: int16) = Doc.Number(num)

    let numUInt16 (num: uint16) = Doc.Number(num)

    let numInt8 (num: int8) = Doc.Number(num)

    let numUInt8 (num: uint8) = Doc.Number(num)

    let numInt64 (num: int64) = Doc.Number(num)
    
    let numUInt64 (num: uint64) = Doc.Number(num)

    let numDouble (num: double) = Doc.Number(num)

    let numSingle (num: single) = Doc.Number(num)

    let numDecimal (num: decimal) = Doc.Number(num)

    let timespan (ts: TimeSpan) = Doc.Timespan(ts)

    let timestamp (dt: DateTime) = Doc.Timestamp(dt)

    let (<->) (left: Doc) (right: Doc) = left.Cat(right)

    let hcat (docs: seq<Doc>) = Doc.Cat(docs)

    let (<+>) (left: Doc) (right: Doc) = left.Sep(right)

    let hsep (docs: seq<Doc>) = Doc.Sep(docs)

    let (<|-|>) (left: Doc) (right: Doc) = left.CatOrAbove(right)

    let cat (docs: seq<Doc>) = Doc.CatOrAbove(docs)

    let (<|+|>) (left: Doc) (right: Doc) = left.SepOrAbove(right)

    let sep (docs: seq<Doc>) = Doc.SepOrAbove(docs)

    let (<^>) (top: Doc) (bottom: Doc) = top.Above(bottom)

    let vcat (docs: seq<Doc>) = Doc.JoinAbove(docs, empty)

    let hcatJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinCat(docs, sep)

    let hsepJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinSep(docs, sep)

    let catJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinCatOrAbove(docs, sep)

    let sepJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinSepOrAbove(docs, sep)

    let vcatJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinAbove(docs, sep)

    let wrap (amount: uint32) (doc: Doc) = doc.Break(amount)

    let wrapAll (amount: uint32) (doc: Doc) = doc.BreakAll(amount)

    let indent (amount: uint32) (doc: Doc) = doc.Indent(amount)

    let indentAll (amount: uint32) (doc: Doc) = doc.IndentAll(amount)

    let inParens (doc: Doc) = Doc.InParens(doc)

    let comma = Doc.Comma

    let dot = Doc.Dot

    let parenOpen = Doc.ParenOpen

    let parenClose = Doc.ParenClose

    let space = Doc.Space

    let spaces (amount: uint32) = Doc.Spaces(amount)

    let newline = Doc.NewLine



