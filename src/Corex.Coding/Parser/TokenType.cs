using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public class TokenType
    {
        public TokenType(string name, Func<StringLocation, StringSelection> tryParse)
        {
            Name = name;
            TryParse = tryParse;
        }
        public Func<StringLocation, StringSelection> TryParse { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
