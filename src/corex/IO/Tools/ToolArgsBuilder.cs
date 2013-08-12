using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.IO.Tools
{
    public class ToolArgsBuilder
    {
        public ToolArgsBuilder(List<ToolArgNode> nodes)
            : this()
        {
            Nodes = nodes;
        }
        public ToolArgsBuilder()
        {
            Nodes = new List<ToolArgNode>();
            SwitchString = "/";
            SwitchSeparatorString = "=";
        }

        public string SwitchString { get; set; }
        public string SwitchSeparatorString { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Nodes.ForEachJoin(t => sb.Append(t), () => sb.Append(" "));
            return sb.ToString();
        }

        void AddNode(ToolArgNode node)
        {
            QuoteIfNeeded(node);
            Nodes.Add(node);
        }

        private void QuoteIfNeeded(ToolArgNode node)
        {
            if (node.Value == null || node.ValueQuotes != null)
                return;
            if (!node.Value.Contains(" "))
                return;
            node.ValueQuotes = "\"";

        }
        public ToolArgsBuilder AddSwitch(string name)
        {
            AddNode(new ToolArgNode { Switch = SwitchString, Name = name });
            return this;
        }
        public ToolArgsBuilder AddSwitchOption(string name, string value)
        {
            AddNode(new ToolArgNode { Switch = SwitchString, Name = name, Value = value });
            return this;
        }
        public ToolArgsBuilder AddOption(string name, string value)
        {
            AddNode(new ToolArgNode { Name = name, Value = value, Seperator = SwitchSeparatorString });
            return this;
        }
        public ToolArgsBuilder AddCommand(string value)
        {
            AddNode(new ToolArgNode { Value = value });
            return this;
        }


        public List<ToolArgNode> Nodes { get; private set; }
    }
}
