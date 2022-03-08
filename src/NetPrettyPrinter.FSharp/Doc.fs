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

namespace NetPrettyPrint.FSharp

module Doc =
    open NetPrettyPrint

    let text (content: string) = Doc.Text(content)

    let (<>) (left: Doc) (right: Doc) = left.Cat(right)

    let hcat (docs: seq<Doc>) = Doc.Cat(docs)

    let (<+>) (left: Doc) (right: Doc) = left.Sep(right)

    let hsep (docs: seq<Doc>) = Doc.Sep(docs)

    let (<||>) (left: Doc) (right: Doc) = left.CatOrAbove(right)

    let cat (docs: seq<Doc>) = Doc.CatOrAbove(docs)

    let (<|+|>) (left: Doc) (right: Doc) = left.SepOrAbove(right)

    let sep (docs: seq<Doc>) = Doc.SepOrAbove(docs)

    let (<^>) (top: Doc) (bottom: Doc) = top.Above(bottom)

    let vcat (docs: seq<Doc>) = failwith "NYI" // TODO

    let hcatJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinCat(docs, sep)

    let hsepJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinSep(docs, sep)

    let catJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinCatOrAboce(docs, sep)

    let sepJoin (sep: Doc) (docs: seq<Doc>) = Doc.JoinSepOrAbove(docs, sep)
