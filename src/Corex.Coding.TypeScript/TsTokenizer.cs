using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser.Parser;

namespace TypeScriptParser
{
    class TsTokenizer
    {
        public static List<Token> Tokenize(string code)
        {
            var tokenTypes = TsTokenTypes.TypeScript;
            var loc2 = new StringLocation(code, 0);


            var tokens = new List<Token>();
            while (!loc2.IsAtEnd)
            {
                var token = NextToken(loc2, tokenTypes);
                if (token == null)
                    throw new Exception("Cannot parse: {" + loc2.Select(30).Text + "}");
                tokens.Add(token);
                loc2 = token.Selection.End;
            }
            tokens = tokens.Where(t => t.IsNot(tokenTypes.Whitespace)).ToList();
            return tokens;
        }

        static Token NextToken(StringLocation loc, TsTokenTypes tokenTypes)
        {
            foreach (var tt in tokenTypes.All)
            {
                var sel = tt.TryParse(loc);
                if (!sel.IsEmpty)
                    return new Token(tt, sel);
            }
            return null;
        }


    }
}
