using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public class StringLocation
    {
        public StringLocation(string s, int index)
        {
            Source = s;
            Index = index;
        }

        TextPosition _Position;
        public TextPosition Position
        {
            get
            {
                if (_Position == null)
                    _Position = CalcPosition(Source, Index);
                return _Position;
            }
        }

        private static TextPosition CalcPosition(string Source, int Index)
        {
            var line = 1;
            var col = 1;
            for (var i = 0; i < Index; i++)
            {
                var ch = Source[i];
                if (ch == '\n')
                {
                    line++;
                    col = 1;
                }
                else
                {
                    col++;
                }
            }
            return new TextPosition(line, col);
        }


        public string GetLineText()
        {
            var pos = Position;
            if (pos == null)
                return null;

            var line = 1;
            var col = 1;
            var i = 0;
            while (line < pos.Line)
            {
                var ch = Source[i];
                if (ch == '\n')
                {
                    line++;
                    col = 1;
                }
                else
                {
                    col++;
                }
                i++;
            }
            var index = Source.IndexOf('\n', i);
            var s = Source.Substring(i, Source.Length - index);
            return s;
        }


        public string Source { get; private set; }
        public int Index { get; private set; }

        public StringSelection SelectStartsWith(string s)
        {
            return Select().ExtendIfAfterEqualsTo(s);
        }
        public StringSelection SelectUntilIndexOf(string s)
        {
            var newIndex = Source.IndexOf(s, Index);
            if (newIndex < 0)
                return Select();
            return Select(newIndex - Index);
        }

        public StringSelection SelectStartsWithRegex(Regex regex)
        {
            var match = regex.Match(Source, Index);
            if (!match.Success)
                return Select();
            if (match.Index != Index)
                return Select();
            return Select(match.Length);
        }

        public StringSelection Select(int length)
        {
            return StringSelection.TryCreate(this, length);
        }

        public StringSelection Select()
        {
            return Select(0);
        }

        public StringLocation Skip(int length)
        {
            return new StringLocation(Source, Index + length);
        }


        public bool IsValid
        {
            get
            {
                return Index <= Source.Length;
            }
        }

        public bool IsAtEnd
        {
            get
            {
                return Index == Source.Length;
            }
        }

    }
}
