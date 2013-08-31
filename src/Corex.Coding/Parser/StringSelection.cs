using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public struct StringSelection
    {
        StringLocation _Start;
        public StringLocation Start { get { return _Start; } }
        int _Length;
        string _Text;

        StringLocation _End;
        public StringLocation End { get { return _End; } }

        public bool IsError
        {
            get
            {
                return _Length < 0 || _Start == null;
            }
        }
        public bool IsValid
        {
            get
            {
                return !IsError;
            }
        }
        public StringSelection(StringLocation start, int length)
        {
            if (start == null)
                throw new ArgumentNullException("start");
            _Start = start;
            _Length = length;
            _Text = null;
            if (_Length < 0)
            {
                _End = null;
            }
            else
            {
                _End = start.Skip(length);
                if (!_End.IsValid)
                    throw new Exception("String selection out of bounds");
            }
        }
        public StringSelection(StringLocation start, StringLocation end)
        {
            if (start == null)
                throw new ArgumentNullException("start");
            if (end == null)
                throw new ArgumentNullException("end");
            _Start = start;
            _End = end;
            _Length = _End.Index - _Start.Index;
            _Text = null;
            if (!_Start.IsValid && !_End.IsValid)
                throw new Exception("String selection out of bounds");
        }


        public static StringSelection TryCreate(StringLocation start, int length)
        {
            if (!start.IsValid)
                return new StringSelection(start, -1);
            var end = start.Skip(length);
            if (!end.IsValid)
                return new StringSelection(start, -1);
            return new StringSelection(start, end);
        }
        public string Text
        {
            get
            {
                if (_Text == null && IsValid)
                    _Text = _Start.Source.Substring(_Start.Index, _Length);
                return _Text;
            }
        }
        public override string ToString()
        {
            if (IsError)
                return "ERROR Selection";
            return String.Format("Length={0}, Index={1}, \"{2}\"", _Length, _Start.Index, Text);
        }
        public bool IsEmpty { get { return _Length == 0; } }


        public StringSelection After(int length)
        {
            if (IsError)
                return this;
            return _End.Select(length);
        }
        public StringSelection ExtendCharsAsLongAs(Func<char, bool> predicate)
        {
            if (IsError)
                return this;
            return ExtendCharsUntil(ch => !predicate(ch));
        }
        public StringSelection ExtendCharsUntil(Func<char, bool> predicate)
        {
            if (IsError)
                return this;
            var length = 0;
            var sel = this;
            while (!sel.IsExtendedToEnd)
            {
                sel = sel.After(1);
                if (sel.IsError)
                    break;
                if (predicate(sel.Text[0]))
                    break;
                length++;
            }
            if (length == 0)
                return this;
            return ExtendBy(length);
        }
        public StringSelection ExtendIfAfterEqualsTo(string s)
        {
            if (IsError)
                return this;
            var sel = After(s.Length);
            if (sel.Text == s)
                return ExtendBy(s.Length);
            return this;
        }
        public StringSelection ExtendUntil(string s)
        {
            if (IsError)
                return this;
            var sel = _End.SelectUntilIndexOf(s);
            return Include(sel);
        }
        public StringSelection ExtendUntilIncluding(string s)
        {
            if (IsError)
                return this;
            return ExtendUntil(s).ExtendIfAfterEqualsTo(s);
        }

        private StringSelection Include(StringSelection sel)
        {
            if (IsError)
                return this;
            var sel1 = this;
            var sel2 = sel;
            var index = sel1._Start.Index;
            if (sel2._Start.Index < sel1._Start.Index)
                index = sel2._Start.Index;
            var endIndex = sel1._End.Index;
            if (sel2._End.Index > sel1._End.Index)
                endIndex = sel2._End.Index;
            return _Start.Source.Select(index, endIndex - index);
        }
        public StringSelection ExtendBy(int offset)
        {
            if (IsError)
                return this;
            return new StringSelection(_Start, _Length + offset);
        }
        public bool IsExtendedToEnd
        {
            get
            {
                if (IsError)
                    return false;
                return End.Index == _Start.Source.Length;
            }
        }
    }
}
