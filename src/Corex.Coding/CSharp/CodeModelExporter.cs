using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace Corex.CodingTools.CSharp
{
    public class CodeModelExporter
    {
        public CodeModelExporter()
        {
            ExportXmlDoc = true;
            ExportXmlDocRemarks = true;
        }
        HashSet<string> keywords = new HashSet<string>
        {
            "namespace","using",
            "object",
            "delegate", "event", "class", "struct", "interface",
            "is",
            "switch",
            "true", "false","lock"
        };
        private void Write(string s, params object[] args)
        {
            Writer.Write(s, args);
        }
        private void Write(string s)
        {
            Writer.Write(s);
        }
        private void WriteLine()
        {
            Writer.WriteLine();
        }
        private void WriteLine(string s, params object[] args)
        {
            Writer.WriteLine(s, args);
        }
        private void WriteLine(string s)
        {
            Writer.WriteLine(s);
        }
        private void BeginBlock()
        {
            WriteLine("{");
            Writer.Indent++;
        }
        private void EndBlock()
        {
            Writer.Indent--;
            WriteLine("}");
        }

        string Identifier(string s)
        {
            if (keywords.Contains(s))
                return "@" + s;
            s = s.Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_");
            return s;
        }
        public void Export(Class ce)
        {
            WriteLine("#region {0}", ce.Name);
            ce.Attributes.ForEach(Export);
            Write("public partial class {0}", Identifier(ce.Name));
            if (ce.BaseClass != null)
                Write(" : {0}", Class(ce.BaseClass));
            WriteLine();
            BeginBlock();
            ce.Methods.ForEach(Export);
            ce.Properties.ForEach(Export);
            EndBlock();
            WriteLine("#endregion");

        }


        private void Export(Property pe)
        {
            ExportXmlDoc2(pe);
            pe.Attributes.ForEach(Export);
            WriteLine("public {0} {1} {{get;set;}}", Class(pe.Type), Identifier(pe.Name));
        }


        string Class(Class ce)
        {
            if (ce.GenericArguments.Count > 0)
            {
                var s = ce.Name.Substring(0, ce.Name.IndexOf("`"));
                if (ce.GenericArguments.IsNotNullOrEmpty())
                    s += "<" + ce.GenericArguments.Select(Class).StringJoin(", ") + ">";
            }
            return ce.Name;
        }


        public bool ExportXmlDoc { get; set; }
        public bool ExportXmlDocRemarks { get; set; }

        void WriteXmlDoc(string ss)
        {
            ss.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ForEach(t =>
            {
                var s = t.Trim();
                if (s.IsNotNullOrEmpty())
                    WriteLine("/// " + s);
            });

        }
        void ExportXmlDoc2(Member me)
        {
            if (ExportXmlDoc)
            {
                if (me.Summary != null)
                {
                    WriteLine("/// <summary>");
                    WriteXmlDoc(me.Summary);
                    //WriteLine("/// " + me.Summary.Trim());
                    WriteLine("/// </summary>");
                }
                if (ExportXmlDocRemarks && me.Remarks != null)
                {
                    //WriteLine("/// <remarks>");
                    WriteXmlDoc(me.Remarks);
                    //WriteLine("/// </remarks>");
                }
            }
        }

        void Export(Attribute att)
        {
            Write("[{0}", att.Name);
            if (att.Parameters.Count > 0 || att.NamedParamters.Count > 0)
            {
                Write("(");
                att.Parameters.ForEachJoin(Write, WriteComma);
                if (att.Parameters.Count > 0 && att.NamedParamters.Count > 0)
                    Write(", ");
                att.NamedParamters.ForEachJoin(t => Write("{0}={1}", t.Key, t.Value), WriteComma);
                Write(")");
            }
            WriteLine("]");
        }
        private void Export(Method me)
        {
            ExportXmlDoc2(me);
            me.Attributes.ForEach(Export);
            Write("public {0}{1} {2}(", me.IsStatic ? "static " : "", me.Type == null ? "void" : Class(me.Type), Identifier(me.Name));
            me.Parameters.ForEachJoin(Export, WriteComma);
            Write(")");
            Write("{");
            //BeginBlock();
            if (me.Type != null && me.Type.Name != "void")
            {
                if (me.Type.Name == "bool")
                    Write("return false;");
                else
                    Write("return null;", Class(me.Type));
            }
            WriteLine("}");
            //EndBlock();

        }

        public void Export(Assembly asm)
        {
            asm.Usings.ForEach(t => WriteLine("using {0};", t));
            WriteLine("namespace {0}", asm.Namespace);
            BeginBlock();
            asm.Classes.ForEach(Export);
            EndBlock();
        }

        private void Export(Parameter prm)
        {
            Write("{0} {1}", Class(prm.Type), Identifier(prm.Name));
            if (prm.IsOptional)
            {
                if (prm.Type.Name == "bool")
                    Write("=false");
                else
                    Write("=null");
            }
        }

        public IndentedTextWriter Writer { get; set; }

        void WriteComma()
        {
            Write(", ");
        }
    }
}
