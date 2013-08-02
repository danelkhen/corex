using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Corex.Reflection;
using System.Collections;

namespace Corex.IO.Tools
{
    public class ToolArgsParser
    {

        IEnumerable<ToolArgNode> Parse(string argsText)
        {
            var tokenizer = new ToolArgsTokenizer();
            var args = tokenizer.Tokenize(argsText);
            return Parse(args);
        }

        public IEnumerable<ToolArgNode> Parse(string[] args)
        {
            foreach (var arg in args)
            {
                var arg2 = arg;
                if (arg2.IsNullOrEmpty())
                    continue;
                var node = new ToolArgNode();
                var switchChars = "/-";
                if (arg.StartsWith("@"))
                {
                    var filename = arg.Substring(1);
                    filename = TrimQuotesIfNeeded(filename);
                    var argsFromFile = File.ReadAllText(filename);
                    var tokens = Parse(argsFromFile);
                    foreach (var token in tokens)
                        yield return token;
                    continue;
                }
                if (arg2.StartsWithAnyChar(switchChars))
                {
                    node.Switch = arg.TakeWhile(t => switchChars.Contains(t)).StringConcat();
                    arg2 = arg2.Substring(node.Switch.Length);
                }
                if (IsQuoted(arg2))
                {
                    node.ValueQuotes = arg2[0].ToString();
                    node.Value = arg2.SubstringBetween(1, arg2.Length - 1);
                }
                else
                {
                    var sepChars = ":=";
                    var sepIndex = arg2.IndexOfAnyChar(sepChars);
                    if (sepIndex >= 0)
                    {
                        node.Name = arg2.SubstringBetween(0, sepIndex);
                        node.Seperator = arg2[sepIndex].ToString();
                        node.Value = arg2.Substring(sepIndex + 1);
                    }
                    else
                    {
                        if (node.Switch != null)
                            node.Name = arg2;
                        else
                            node.Value = arg2;
                    }
                    if (node.Value != null && IsQuoted(node.Value))
                    {
                        node.ValueQuotes = node.Value[0].ToString();
                        node.Value = node.Value.SubstringBetween(1, node.Value.Length - 1);
                    }
                }
                yield return node;
            }
        }

        private string TrimQuotesIfNeeded(string arg)
        {
            if (!IsQuoted(arg))
                return arg;
            return arg.SubstringBetween(1, arg.Length - 1);
        }

        private bool IsQuoted(string arg)
        {
            return arg.StartsAndEndsWith("\"") || arg.StartsAndEndsWith("\'");
        }

        void Warn(string msg, params object[] args)
        {
            Console.WriteLine(msg, args);
        }

    }



}
