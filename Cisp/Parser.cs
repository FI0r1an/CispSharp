using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cisp
{
    struct ParserState
    {
        public int Index;
        public int LIndex;
        public int Line;
        public string Source;
        public ParserState(string source)
        {
            Source = source;
            Index = 0;
            LIndex = Line = 1;
        }
        public string CharAt(int idx)
        {
            return (idx + 1) <= Source.Length ? Source.Substring(idx, 1) : "\0";
        }
    }
    class Parser
    {
        private ParserState ps;
        private Stack stack = new Stack();
        public Parser(ParserState ps)
        {
            this.ps = ps;
        }
        void Assert(bool b)
        {
            if (!b) throw new TException(string.Format("Wrong character at line {0}, position {1}", ps.Line, ps.LIndex));
        }
        void Assert(bool b, string s)
        {
            if (!b) throw new TException(string.Format(s + " at line {0}, position {1}", ps.Line, ps.LIndex));
        }
        string Next() { string old = Current();  ps.Index++; ps.LIndex++; return old; }
        void ToZero() { ps.Line++; ps.LIndex = 1; }
        void SkipWhitespace()
        {
            while (Util.IsWhitespace(Current()))
            {
                if (Util.IsLine(Current()))
                {
                    string old = Current();
                    if (Util.IsLine(LookAhead()) && (LookAhead() != old)) Next();
                    ToZero();
                }
                Next();
            }
        }
        string Current() { return ps.CharAt(ps.Index); }
        string LookAhead() { return ps.CharAt(ps.Index + 1); }
        bool IsEof() { return (ps.Index + 1) >= ps.Source.Length; }
        bool NotStart(string str)
        {
            switch (str[0])
            {
                case '(':
                    return false;
            }
            return true;
        }
        Token ReadString()
        {
            Token tk = new Token();
            tk.Type = TokenType.String;
            string result = "";
            string start = Current();
            Next();
            while (Current() != start && Current() != "\'" && Current() != "\"")
            {
                Assert(!IsEof());
                result += Current();
                Assert(!Util.IsLine(Current()));
                Next();
            }
            tk.Value = Regex.Unescape(result);
            return tk;
        }
        Token ReadNumber()
        {
            Token tk = new Token();
            tk.Type = TokenType.Number;
            string result = "";
            if (Current() == "+" || Current() == "-") result += Next();
            while (Util.IsDigit(Current()) || Current() == ".")
            {
                result += Current();
                Assert(!Util.IsLine(Current()));
                Next();
            }
            double r;
            double.TryParse(result, out r);
            tk.Value = r;
            return tk;
        }
        Token ReadName()
        {
            Token tk = new Token();
            tk.Type = TokenType.Name;
            string result = "";
            while (!Util.IsWhitespace(Current()) && NotStart(Current()))
            {
                result += Current();
                Assert(!Util.IsWhitespace(Current()));
                if (IsEof()) break;
                Next();
            }
            tk.Value = result;
            return tk;
        }
        Token ReadList()
        {
            Token tk = new Token();
            tk.Type = TokenType.List;
            Queue result = new Queue();
            
            Next();
            while (true)
            {
                SkipWhitespace();
                if (Current() == ")") break;
                Assert(!IsEof());
                Token c = ReadToken();
                result.Enqueue(c);
                if (Current() == ")") break;
                Assert(Util.IsWhitespace(Current()));
            }
            Assert(result.Count > 0, "Got a empty list");
            Next();
            tk.Value = result;
            return tk;
        }
        public Token ReadToken()
        {
            string c = Current();
            if (c == "(")
            {
                return ReadList();
            }
            else if (Util.IsQuote(c))
            {
                return ReadString();
            }
            else if (Util.IsDigit(c) || c == "+" || c == "-" || c == ".")
            {
                return ReadNumber();
            }
            return ReadName();
        }
    }
}
