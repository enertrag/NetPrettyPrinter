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
using System.Globalization;
using System.Linq;
using Tok = NetPrettyPrinter.Token;

namespace NetPrettyPrinter
{
    public class Doc
    {
        private readonly Elem _elem;

        private Doc(Elem elem)
        {
            _elem = elem;
        }

        public string PrettyPrint(uint lineWidth = 80)
        {
            var pp = new PrettyPrint(Convert.ToInt32(lineWidth));
            pp.PrettyPrintToken(Tok.Begin(0));
            var stack = new Stack<Elem>();
            stack.Push(_elem);
            while(stack.Count != 0)
            {
                var elem = stack.Pop();
                switch(elem)
                {
                    case ElemSingle single:
                        var token = single.Tok;
                        pp.PrettyPrintToken(token);
                        break;
                    case ElemConcat concat:
                        stack.Push(concat.Right);
                        stack.Push(concat.Left);
                        break;
                }
            }

            pp.PrettyPrintToken(Tok.End());
            pp.PrettyPrintToken(Tok.Eof());

            return pp.GetValue();
        }

        public static Doc Text(string content) => NewDoc(Singleton(Tok.Text(content)));
        public static Doc Number(int num) => num.ToString();
        public static Doc Number(uint num) => num.ToString();
        public static Doc Number(short num) => num.ToString();
        public static Doc Number(ushort num) => num.ToString();
        public static Doc Number(sbyte num) => num.ToString();
        public static Doc Number(byte num) => num.ToString();
        public static Doc Number(long num) => num.ToString();
        public static Doc Number(ulong num) => num.ToString();
        public static Doc Number(float num) => num.ToString(CultureInfo.InvariantCulture);
        public static Doc Number(double num) => num.ToString(CultureInfo.InvariantCulture);
        public static Doc Number(decimal num) => num.ToString(CultureInfo.InvariantCulture);
        public static Doc Timespan(TimeSpan span) => span.ToString();
        public static Doc Timestamp(DateTime timestamp) => timestamp.ToString("s", CultureInfo.InvariantCulture);

        public static implicit operator Doc(string content) => Text(content);
        public static implicit operator Doc(int num) => Number(num);
        public static implicit operator Doc(uint num) => Number(num);
        public static implicit operator Doc(short num) => Number(num);
        public static implicit operator Doc(ushort num) => Number(num);
        public static implicit operator Doc(sbyte num) => Number(num);
        public static implicit operator Doc(byte num) => Number(num);
        public static implicit operator Doc(long num) => Number(num);
        public static implicit operator Doc(ulong num) => Number(num);
        public static implicit operator Doc(float num) => Number(num);
        public static implicit operator Doc(double num) => Number(num);
        public static implicit operator Doc(decimal num) => Number(num);
        public static implicit operator Doc(TimeSpan span) => Timespan(span);
        public static implicit operator Doc(DateTime timestamp) => Timestamp(timestamp);

        /// <summary>
        ///     Besides each other without space.
        /// </summary>
        /// <param name="doc">right document</param>
        /// <returns>composed document</returns>
        public Doc Cat(Doc doc) => NewDoc(!doc._elem.IsVisible ? _elem : _elem.Concat(doc._elem));

        /// <summary>
        ///     Besides each other without space.
        /// </summary>
        /// <param name="left">left document</param>
        /// <param name="right">right document</param>
        /// <returns>composed document</returns>
        public static Doc operator +(Doc left, Doc right) => left.Cat(right);

        /// <summary>
        ///     Besides for a sequence without space.
        /// </summary>
        /// <param name="docs">documents to put besides each other</param>
        /// <returns>composed document</returns>
        public static Doc Cat(IEnumerable<Doc> docs) => JoinDocs(docs, (agg, doc) => agg.Cat(doc));

        /// <summary>
        ///     Besides each other with space.
        /// </summary>
        /// <param name="doc">right document</param>
        /// <param name="spaces">number of separating spaces</param>
        /// <returns>composed document</returns>
        public Doc Sep(Doc doc, uint spaces = 1) =>
            NewDoc(!doc._elem.IsVisible ? _elem : _elem.Concat(Spaces(spaces)._elem).Concat(doc._elem));

        /// <summary>
        ///     Besides each other without space.
        /// </summary>
        /// <param name="left">left document</param>
        /// <param name="right">right document</param>
        /// <returns>composed document</returns>
        public static Doc operator %(Doc left, Doc right) => left.Sep(right);

        /// <summary>
        ///     Besides for a sequence with space.
        /// </summary>
        /// <param name="docs">documents to put besides each other</param>
        /// <param name="spaces">number of separating spaces</param>
        /// <returns>composed document</returns>
        public static Doc Sep(IEnumerable<Doc> docs, uint spaces = 1) => JoinDocs(docs, (agg, doc) => agg.Sep(doc, spaces));

        /// <summary>
        ///     Besides without space or one above the other.
        /// </summary>
        /// <param name="doc">right or lower document</param>
        /// <returns>composed document</returns>
        public Doc CatOrAbove(Doc doc) =>
            NewDoc(!doc._elem.IsVisible ? _elem : _elem.Concat(Singleton(Tok.Break(0))).Concat(doc._elem));

        /// <summary>
        ///     Besides without space or one above the other.
        /// </summary>
        /// <param name="left">left or  document</param>
        /// <param name="right">right or lower document</param>
        /// <returns>composed document</returns>
        public static Doc operator |(Doc left, Doc right) => left.CatOrAbove(right);

        /// <summary>
        ///     Besides without space or above each other for a sequence.
        /// </summary>
        /// <param name="docs">documents to put besides or above each other</param>
        /// <returns>composed document</returns>
        public static Doc CatOrAbove(IEnumerable<Doc> docs) => JoinDocs(docs, (agg, doc) => agg.CatOrAbove(doc));

        /// <summary>
        ///     Besides with space or one above the other.
        /// </summary>
        /// <param name="doc">right or lower document</param>
        /// <returns>composed document</returns>
        public Doc SepOrAbove(Doc doc) =>
            NewDoc(!doc._elem.IsVisible ? _elem : _elem.Concat(Singleton(Tok.Break())).Concat(doc._elem));

        /// <summary>
        ///     Besides with space or one above the other.
        /// </summary>
        /// <param name="left">left or  document</param>
        /// <param name="right">right or lower document</param>
        /// <returns>composed document</returns>
        public static Doc operator &(Doc left, Doc right) => left.SepOrAbove(right);

        /// <summary>
        ///     Besides with space or above each other for a sequence.
        /// </summary>
        /// <param name="docs">documents to put besides or above each other</param>
        /// <returns>composed document</returns>
        public static Doc SepOrAbove(IEnumerable<Doc> docs) => JoinDocs(docs, (agg, doc) => agg.SepOrAbove(doc));

        /// <summary>
        ///     Put document above each other
        /// </summary>
        /// <param name="doc">lower document</param>
        /// <returns>composed document</returns>
        public Doc Above(Doc doc) =>
            _elem.IsVisible && doc._elem.IsVisible
                ? NewDoc(_elem.Concat(Singleton(Tok.LineBreak())).Concat(doc._elem))
                : NewDoc(_elem.Concat(doc._elem));

        /// <summary>
        ///     Put document above each other
        /// </summary>
        /// <param name="top">upper document</param>
        /// <param name="bottom">lower document</param>
        /// <returns>composed document</returns>
        public static Doc operator ^(Doc top, Doc bottom) => top.Above(bottom);

        public static Doc JoinCat(IEnumerable<Doc> docs, Doc sep) => JoinDocs(docs, (agg, doc) => agg + sep + doc);

        public static Doc JoinSep(IEnumerable<Doc> docs, Doc sep) => JoinDocs(docs, (agg, doc) => agg + sep % doc);

        public static Doc JoinCatOrAbove(IEnumerable<Doc> docs, Doc sep) => JoinDocs(docs, (agg, doc) => agg + sep | doc);

        public static Doc JoinSepOrAbove(IEnumerable<Doc> docs, Doc sep) => JoinDocs(docs, (agg, doc) => agg + sep & doc);

        public static Doc JoinAbove(IEnumerable<Doc> docs, Doc sep) => JoinDocs(docs, (agg, doc) => agg + sep ^ doc);

        public Doc Break(uint amount) =>
            NewDoc(Singleton(Tok.Begin(Convert.ToInt32(amount))).Concat(_elem).Concat(Singleton(Tok.End())));

        public static Doc Break(uint amount, Doc doc) => doc.Break(amount);

        public Doc Break() => Break(Tok.DefaultIndent);

        public static Doc Break(Doc doc) => doc.Break();

        public Doc BreakAll(uint amount) => NewDoc(Singleton(Tok.Begin(Convert.ToInt32(amount), BreakType.Consistent))
            .Concat(_elem)
            .Concat(Singleton(Tok.End())));

        public static Doc BreakAll(uint amount, Doc doc) => doc.BreakAll(amount);

        public static Doc BreakAll(Doc doc) => BreakAll(Tok.DefaultIndent, doc);

        public Doc Indent(uint amount) => NewDoc(Singleton(Tok.Break(offset: Convert.ToInt32(amount)))
            .Concat(Singleton(Tok.Begin(0))).Concat(_elem)
            .Concat(Singleton(Tok.End())));

        public static Doc Indent(uint amount, Doc doc) => doc.Indent(amount);

        public Doc Indent() => Indent(Tok.DefaultIndent);
        public static Doc Indent(Doc doc) => doc.Indent();

        public Doc IndentAll(uint amount) => NewDoc(Singleton(Tok.Break(offset: Convert.ToInt32(amount)))
            .Concat(Singleton(Tok.Begin(0, BreakType.Consistent))).Concat(_elem)
            .Concat(Singleton(Tok.End())));

        public static Doc IndentAll(uint amount, Doc doc) => doc.IndentAll(amount);

        public Doc IndentAll() => IndentAll(Tok.DefaultIndent);

        public static Doc IndentAll(Doc doc) => doc.IndentAll();

        public static Doc InParens(Doc doc) => "(" + doc + ")";

        public static Doc Comma = ",";
        public static Doc Dot = ".";
        public static Doc ParenOpen => "(";
        public static Doc ParenClose => ")";
        public static Doc Space => Spaces(1);
        public static Doc Spaces(uint amount) => new string(' ', Convert.ToInt32(amount));
        public static Doc NewLine => NewDoc(Singleton(Tok.LineBreak()));
        private static readonly Elem EmptyElem = new ElemEmpty();
        public static readonly Doc Empty = NewDoc(EmptyElem);
        public static Doc ToDoc(bool visible, string text) => visible ? text : Empty;

        private static Doc NewDoc(Elem elem) => new Doc(elem);

        private static Elem Singleton(Tok token) => new ElemSingle(token);

        private static Doc JoinDocs(IEnumerable<Doc> docs, Func<Doc, Doc, Doc> joinFunc)
        {
            var docList = docs.ToList();
            return docList.Count switch
            {
                0 => Empty,
                1 => docList.First(),
                _ => docList.Aggregate(joinFunc)
            };
        }

        private abstract class Elem
        {
            public abstract Elem Concat(Elem elem);
            public abstract bool IsVisible { get; }
        }

        private class ElemEmpty : Elem
        {
            public override Elem Concat(Elem elem) => elem;
            public override bool IsVisible => false;
        }

        private class ElemConcat : Elem
        {
            public ElemConcat(Elem left, Elem right)
            {
                Left = left;
                Right = right;
                IsVisible = left.IsVisible || right.IsVisible;
            }

            public Elem Left { get; }
            public Elem Right { get; }

            public override Elem Concat(Elem elem) => new ElemConcat(this, elem);
            public override bool IsVisible { get; }
        }

        private class ElemSingle : Elem
        {
            public ElemSingle(Tok tok)
            {
                Tok = tok;
                IsVisible = tok switch
                {
                    Text text => text.Content.Length > 0,
                    _ => false
                };
            }

            public Tok Tok { get; }

            public override Elem Concat(Elem elem) => new ElemConcat(this, elem);
            public override bool IsVisible { get; }
        }
    }
}