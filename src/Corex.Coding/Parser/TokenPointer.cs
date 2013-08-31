using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public class TokenPointer
    {
        public TokenPointer(List<Token> list, int index)
        {
            List = list;
            Index = index;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public int Index { get; private set; }
        public List<Token> List { get; private set; }
        public Token Value
        {
            get
            {
                return List.TryGet(Index);
            }
        }

        public string TextArea
        {
            get
            {
                return GetTextArea(Index - 10, 20);
            }
        }

        private string GetTextArea(int startIndex, int length)
        {
            if (startIndex < 0)
                startIndex = 0;
            if (startIndex + length >= List.Count)
                length = List.Count - startIndex;
            var x = TryGet(startIndex).SelfAndNextSibilings().Take(length).Select(t => t.Value.Text).ToArray();
            var s = String.Concat(x);
            return s;
        }
        public TokenPointer Next2 { get { return Next(1); } }
        public TokenPointer Prev2 { get { return Prev(1); } }
        public TokenPointer Next(int offset = 1) { return TryGet(Index + offset); }
        public TokenPointer Prev(int offset = 1) { return TryGet(Index - offset); }
        public IEnumerable<TokenPointer> PrevSibilings()
        {
            return PrevFromIndex(Index - 1);
        }
        public IEnumerable<TokenPointer> NextSibilings()
        {
            return NextFronIndex(Index + 1);
        }
        public IEnumerable<TokenPointer> NextFronIndex(int i)
        {
            while (i < List.Count)
            {
                var x = TryGet(i);
                if (x == null)
                    yield break;
                yield return x;
                i++;
            }
        }
        TokenPointer TryGet(int index)
        {
            return new TokenPointer(List, index);
        }
        public IEnumerable<TokenPointer> PrevFromIndex(int i)
        {
            while (i >= 0)
            {
                var x = TryGet(i);
                if (x == null)
                    yield break;
                yield return x;
                i--;
            }
        }
        public IEnumerable<TokenPointer> SelfAndNextSibilings()
        {
            return NextFronIndex(Index);
        }

        [DebuggerStepThrough]
        public TokenPointer Verify(Func<Token, bool> func)
        {
            if (!func(Value))
                throw new Exception();
            return this;
        }

    }
}
