// Copyright(c) 2020, Enertrag
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

using System;
using System.Collections.Generic;
using System.Text;

namespace NetPrettyPrinter
{
    public class TokenPrinter
    {
        public int Space { get; private set; }
        private readonly int _margin;
        private readonly StringBuilder _output = new StringBuilder();
        private readonly Stack<PrintStackEntry> _printStack = new Stack<PrintStackEntry>();

        public TokenPrinter(int lineWidth)
        {
            Space = lineWidth;
            _margin = lineWidth;
        }

        public void Print(Token token, int length)
        {
            switch(token)
            {
                case Begin begin:
                    PrintBegin(length, begin);
                    break;
                case End _:
                    PrintEnd();
                    break;
                case Break @break:
                    PrintBreak(length, @break);
                    break;
                case Text text:
                    PrintText(length, text);
                    break;
                default:
                    throw new Exception("invalid token");
            }
        }

        public void Indent(int amount) => Append(new string(' ', amount));

        public string GetValue() => _output.ToString();

        private void PrintBegin(int length, Begin begin)
        {
            if(length > Space)
            {
                Push(new PrintStackEntry(Space - begin.Offset,
                    begin.BreakType == BreakType.Consistent ? BreakType.Consistent : BreakType.Inconsistent));
            }
            else
            {
                Push(new PrintStackEntry(0, BreakType.Fits));
            }
        }

        private void PrintEnd() => Pop();

        private void PrintBreak(int length, Break @break)
        {
            var block = Top();
            switch(block.BreakType)
            {
                case BreakType.Fits:
                    Space -= @break.BlankSpace;
                    Indent(@break.BlankSpace);
                    break;
                case BreakType.Consistent:
                    Space = block.Offset - @break.Offset;
                    NewLine(_margin - Space);
                    break;
                case BreakType.Inconsistent:
                    if(length > Space)
                    {
                        Space = block.Offset - @break.Offset;
                        NewLine(_margin - Space);
                    }
                    else
                    {
                        Space -= @break.BlankSpace;
                        Indent(@break.BlankSpace);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PrintText(int length, Text text)
        {
            if(length > Space)
            {
                throw new Exception("line too long");
            }

            Space -= length;
            Append(text.Content);
        }

        private void Push(PrintStackEntry entry) => _printStack.Push(entry);

        private void Pop() => _printStack.Pop();

        private PrintStackEntry Top() => _printStack.Peek();

        private void NewLine(int amount)
        {
            Append(Environment.NewLine);
            Indent(amount);
        }

        private void Append(string s) => _output.Append(s);
    }
}