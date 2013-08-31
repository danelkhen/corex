using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser.Parser;

namespace TypeScriptParser
{
    static class Extensions
    {
        public static bool IsIdentifier(this Token token, string text)
        {
            return token.Is(TsTokenTypes.TypeScript.Identifier, text);
        }
        public static bool IsIdentifier(this Token token)
        {
            return token.Is(TsTokenTypes.TypeScript.Identifier);
        }
        public static bool IsOperator(this Token token)
        {
            return token.Is(TsTokenTypes.TypeScript.Operator);
        }
        public static bool IsOperator(this Token token, string text)
        {
            return token.Is(TsTokenTypes.TypeScript.Operator, text);
        }

    }
}
