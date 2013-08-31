using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public class Token
    {
        public override string ToString()
        {
            return String.Format("Token {0} '{1}' - at {2}", Type, Text, Selection.Start.Position);
        }
        public StringSelection Selection { get; private set; }

        public Token(TokenType tt, StringSelection sel)
        {
            Type = tt;
            Selection = sel;


        }
        public TokenType Type { get; set; }
        public string Text { get { return Selection.Text; } }

        public bool Is(TokenType type, string text)
        {
            return Is(type) && Text == text;
        }
        public bool Is(params TokenType[] types)
        {
            foreach (var type in types)
                if (Type == type)
                    return true;
            return false;
        }
        public bool IsNot(params TokenType[] types)
        {
            foreach (var type in types)
                if (Type == type)
                    return false;
            return true;
        }

    }
}
