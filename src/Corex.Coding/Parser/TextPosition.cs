using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public class TextPosition
    {
        public int Line { get; set; }
        public int Col { get; set; }

        public TextPosition(int line, int col)
        {
            this.Line = line;
            this.Col = col;
        }
        public override string ToString()
        {
            return String.Format("({0}, {1})", Line, Col);
        }
    }
}
