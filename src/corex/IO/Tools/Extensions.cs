using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO.Tools
{
    public static class Extensions
    {
        public static ToolArgsBuilder Build(this IEnumerable<ToolArgNode> nodes)
        {
            return new ToolArgsBuilder(nodes.ToList());
        }
        public static List<ToolArgNode> AddSwitch(this List<ToolArgNode> list, string name, string switchString = "/")
        {
            list.Add(new ToolArgNode { Switch = switchString, Name = name });
            return list;
        }
        public static List<ToolArgNode> AddSwitchOption(this List<ToolArgNode> list, string name, string value, string switchString = "/")
        {
            list.Add(new ToolArgNode { Switch=switchString, Name = name, Value = value });
            return list;
        }
        public static List<ToolArgNode> AddOption(this List<ToolArgNode> list, string name, string value, string separatorString="=")
        {
            list.Add(new ToolArgNode { Name = name, Value = value, Seperator=separatorString });
            return list;
        }
        public static List<ToolArgNode> AddCommand(this List<ToolArgNode> list, string value)
        {
            list.Add(new ToolArgNode { Value = value });
            return list;
        }
    }
}
