using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Corex.CodingTools.CSharp
{
    class CodeModelImporter
    {

        private static List<List<T>> GetAllCombinations<T>(List<List<T>> list)
        {
            if (list.Count == 0)
                return new List<List<T>>();
            var ops = list[0];
            list = list.Skip(1).ToList();
            if (list.Count == 0)
            {
                var combs2 = ops.Select(t => new List<T> { t }).ToList();
                return combs2;
            }
            var combs = GetAllCombinations(list).ToList();
            var combs3 = new List<List<T>>();
            foreach (var op in ops)
            {
                foreach (var comb in combs)
                {
                    var comb3 = new List<T> { op };
                    comb3.AddRange(comb);
                    combs3.Add(comb3);
                }
            }
            return combs3;
        }
        private static void AddOptionalOverloads(Class ce, Method me, List<Parameter> optionalParameters)
        {
            foreach (var prm in optionalParameters.ToArray())
            {
                var me2 = me.Clone();
                me2.Parameters.Remove(me2.Parameters.Where(t => t.Name == prm.Name).First());
                ce.Methods.Add(me2);
                var op2 = optionalParameters.ToArray().ToList();
                op2.Remove(prm);
                AddOptionalOverloads(ce, me2, op2);
            }
        }

        Dictionary<string, string> HtmlToXmlDoc = new Dictionary<string, string>
        {
            {"code", "c"},
            {"pre", "code"},
            {"p", null},
            {"longdesc", "remarks"},
            {"string", null}
        };
        private void ConvertToXmlDoc(XElement el)
        {
            string newName;
            if (HtmlToXmlDoc.TryGetValue(el.Name.LocalName, out newName))
            {
                if (newName != null)
                {
                    el.Name = newName;
                }
                else
                {
                    foreach (var node in el.Nodes().ToArray())
                    {
                        el.AddAfterSelf(node);
                    }
                    el.Remove();
                }
            }
            foreach (var ch in el.Elements().ToArray())
            {
                ConvertToXmlDoc(ch);
            }
        }

    }



}
