using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Corex.IO.Tools
{
    public class ToolArgsHelpGenerator<T> where T:class, new()
    {
        public ToolArgsHelpGenerator(ToolArgsInfo<T> info)
        {
            Info = info;
        }
        ToolArgsInfo<T> Info;

        public string Generate()
        {
            var sb = new StringWriter();
            Generate(sb);
            return sb.GetStringBuilder().ToString();
        }
        public void Generate(TextWriter writer)
        {
            writer.WriteLine("Usage:");
            if (Info.CommandProperty != null)
            {
                writer.Write("[{0}]", Info.CommandProperty.Name);
            }
            foreach (var pe in Info.SwitchProperties)
            {
                writer.Write(" ");
                var name = Info.GetSwitchNames(pe).First();
                if (name != pe.Name)
                    writer.Write("/[{0} ({1})]", name, pe.Name);
                else
                    writer.Write("/[{0}]", name, pe.Name);
            }
            writer.WriteLine();
        }

    }
}
