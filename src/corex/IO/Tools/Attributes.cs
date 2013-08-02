using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace Corex.IO.Tools
{
    public class ToolArgSwitchAttribute : Attribute
    {
        public ToolArgSwitchAttribute(string name)
        {
            Names = new string[] { name };
        }
        public ToolArgSwitchAttribute(string name1, string name2)
        {
            Names = new string[] { name1, name2 };
        }
        public ToolArgSwitchAttribute()
        {
        }
        public string[] Names { get; set; }
        public string[] Switches { get; set; }
        public string[] Separators { get; set; }
    }
    public class ToolArgCommandAttribute : Attribute
    {
    }




}
