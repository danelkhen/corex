using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO.Tools
{
    public class ToolArgNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Switch { get; set; }
        public string Seperator { get; set; }
        public string ValueQuotes { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Switch);
            sb.Append(Name);
            sb.Append(Seperator);
            sb.Append(ValueQuotes);
            sb.Append(Value);
            sb.Append(ValueQuotes);

            return String.Join("", Switch, Name, Seperator, ValueQuotes, Value, Value);
        }
    }
}
