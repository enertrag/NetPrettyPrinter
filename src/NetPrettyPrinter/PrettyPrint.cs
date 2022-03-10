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
// SERVICES, LOSS OF USE, DATA, OR PROFITS, OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

namespace NetPrettyPrinter;

internal class PrettyPrint
{
    private const int SizeInfinity = int.MaxValue;
    private readonly TokenPrinter _printer;
    private readonly ScanLifo _scanStack;
    private readonly Token[] _token;
    private readonly int[] _size;
    private int _leftTotal;
    private int _rightTotal;
    private int _left;
    private int _right;

    public PrettyPrint(int lineWidth = 80)
    {
        var n = 3 * lineWidth;
        _scanStack = new ScanLifo(n);
        _token = new Token[n];
        _size = new int[n];
        _leftTotal = 1;
        _rightTotal = 1;
        _printer = new TokenPrinter(lineWidth);
    }

    public string GetValue() => _printer.GetValue();

    public void PrettyPrintToken(Token token)
    {
        switch(token)
        {
            case Eof:
                PrettyPrintEof();
                break;
            case Begin begin:
                PrettyPrintBegin(begin);
                break;
            case Terminal end:
                PrettyPrintEnd(end);
                break;
            case Break @break:
                PrettyPrintBreak(@break);
                break;
            case Text text:
                PrettyPrintText(text);
                break;
            default:
                throw new ArgumentOutOfRangeException($"unexpected {token}");
        }
    }

    private void PrettyPrintEof()
    {
        if(!_scanStack.IsEmpty)
        {
            CheckStack(0);
            AdvanceLeft(_token[_left], _size[_left]);
        }

        _printer.Indent(0);
    }

    private void PrettyPrintBegin(Token begin)
    {
        if(_scanStack.IsEmpty)
        {
            _leftTotal = 1;
            _rightTotal = 1;
            _left = 0;
            _right = 0;
        }
        else
        {
            AdvanceRight();
        }

        _token[_right] = begin;
        _size[_right] = -_rightTotal;
        _scanStack.Push(_right);
    }

    private void PrettyPrintEnd(Token end)
    {
        if(_scanStack.IsEmpty)
        {
            _printer.Print(end, 0);
            return;
        }

        AdvanceRight();
        _token[_right] = end;
        _size[_right] = -1;
        _scanStack.Push(_right);
    }

    private void PrettyPrintBreak(Break @break)
    {
        if(_scanStack.IsEmpty)
        {
            _leftTotal = 1;
            _rightTotal = 1;
            _left = 0;
            _right = 0;
        }
        else
        {
            AdvanceRight();
        }

        CheckStack(0);
        _scanStack.Push(_right);
        _token[_right] = @break;
        _size[_right] = -_rightTotal;
        _rightTotal += @break.BlankSpace;
    }

    private void PrettyPrintText(Text text)
    {
        if(_scanStack.IsEmpty)
        {
            _printer.Print(text, text.Length);
            return;
        }

        AdvanceRight();
        _token[_right] = text;
        _size[_right] = text.Length;
        _rightTotal += text.Length;
        CheckStream();
    }

    private void CheckStream()
    {
        while(true)
        {
            if(_rightTotal - _leftTotal <= _printer.Space)
            {
                return;
            }

            if(!_scanStack.IsEmpty)
            {
                if(_left == _scanStack.Bottom)
                {
                    _size[_scanStack.PopBottom()] = SizeInfinity;
                }
            }

            AdvanceLeft(_token[_left], _size[_left]);

            if(_left != _right)
            {
                continue;
            }

            break;
        }
    }

    private void CheckStack(int k)
    {
        while(true)
        {
            if(_scanStack.IsEmpty)
            {
                return;
            }

            var top = _scanStack.Top;
            switch(_token[top])
            {
                case Begin:
                {
                    if(k > 0)
                    {
                        _size[_scanStack.Pop()] = _size[top] + _rightTotal;
                        k -= 1;
                        continue;
                    }

                    break;
                }
                case Terminal:
                    _size[_scanStack.Pop()] = 1;
                    k += 1;
                    continue;
                default:
                {
                    _size[_scanStack.Pop()] = _size[top] + _rightTotal;
                    if(k > 0)
                    {
                        continue;
                    }

                    break;
                }
            }

            break;
        }
    }

    private void AdvanceRight()
    {
        _right = (_right + 1) % _scanStack.Length;
        if(_right == _left)
        {
            throw new Exception("token queue full");
        }
    }

    private void AdvanceLeft(Token token, int length)
    {
        while(true)
        {
            if(length < 0)
            {
                return;
            }

            _printer.Print(token, length);
            _leftTotal +=
                token switch
                {
                    Break @break => @break.BlankSpace,
                    Text => length,
                    _ => throw new ArgumentOutOfRangeException($"unexpected {token}")
                };

            if(_left == _right)
            {
                return;
            }

            _left = (_left + 1) % _scanStack.Length;
            token = _token[_left];
            length = _size[_left];
        }
    }
}