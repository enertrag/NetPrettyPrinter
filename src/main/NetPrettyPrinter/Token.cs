// Copyright(c) 2022, ENERTRAG SE
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
// SERVICES, LOSS OF USE, DATA, OR PROFITS, OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

namespace NetPrettyPrinter;
internal abstract class Token
{
    public const int DefaultIndent = 2;

    public static Token Begin(int offset = DefaultIndent, BreakType breakType = BreakType.Inconsistent) =>
        new Begin(offset, breakType);

    public static Token End() => new Terminal();
    public static Token Break(int blankSpace = 1, int offset = 0) => new Break(blankSpace, offset);
    public static Token Eof() => new Eof();
    public static Token Text(string content) => new Text(content);
    public static Token LineBreak() => new Break(9999);
}

internal class Begin : Token
{
    public Begin(int offset = DefaultIndent, BreakType breakType = BreakType.Inconsistent)
    {
        Offset = offset;
        BreakType = breakType;
    }

    public int Offset { get; }
    public BreakType BreakType { get; }
}

internal class Terminal : Token
{
}

internal class Break : Token
{
    public Break(int blankSpace = 1, int offset = 0)
    {
        BlankSpace = blankSpace;
        Offset = offset;
    }

    public int BlankSpace { get; }
    public int Offset { get; }
}

internal class Eof : Token
{
}

internal class Text : Token
{
    public Text(string content) => Content = content;

    public string Content { get; }

    public int Length => Content.Length;
}