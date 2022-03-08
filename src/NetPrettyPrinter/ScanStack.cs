﻿// Copyright(c) 2020, Enertrag
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

namespace NetPrettyPrinter
    public class ScanStack
    {
        private int _top;
        private int _bottom;
        private readonly int[] _stack;

        public ScanStack(int size)
        {
            _stack = new int[size];
        }

        public bool IsEmpty { get; private set; } = true;

        public int Length => _stack.Length;

        public int Top => IsEmpty ? throw new System.Exception("stack empty") : _stack[_top];

        public int Bottom => IsEmpty ? throw new System.Exception("stack full") : _stack[_bottom];

        public void Push(int x)
        {
            if(IsEmpty)
            {
                IsEmpty = false;
            }
            else
            {
                _top = Inc(_top);
                if(_top == _bottom)
                {
                    throw new System.Exception("stack full");
                }
            }

            _stack[_top] = x;
        }

        public int Pop()
        {
            if(IsEmpty)
            {
                throw new System.Exception("stack empty");
            }

            var result = _stack[_top];
            if(_top == _bottom)
            {
                IsEmpty = true;
            }
            else
            {
                _top = Dec(_top);
            }

            return result;
        }

        public int PopBottom()
        {
            if(IsEmpty)
            {
                throw new System.Exception("stack empty");
            }

            var result = _stack[_bottom];
            if(_top == _bottom)
            {
                IsEmpty = true;
            }
            else
            {
                _bottom = Inc(_bottom);
            }

            return result;
        }

        private int Inc(int index) => (index + 1) % Length;

        private int Dec(int index) => (index - 1) % Length;
    }
}