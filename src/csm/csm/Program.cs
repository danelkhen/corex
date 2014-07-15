using Corex.Helpers;
using Corex.IO.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csm
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                new Program { Args = args }.Run();
                return 0;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        private void Run()
        {
            CsFile = Args[0].ToFileInfo();
            if (!CsFile.Exists && CsFile.Extension != ".cs")
                CsFile = (CsFile.FullName + ".cs").ToFileInfo();
            if (!CsFile.Exists)
                throw new Exception("File not found " + Args[0]);
            Scripts.Add(new Script { File = CsFile, Text = CsFile.ReadAllText() });
            var tempFile = Path.GetTempFileName().ToFileInfo();
            TempDir = tempFile.GetParent().GetDirectory(tempFile.GetNameWithoutExtension());
            TempDir.VerifyExists();
            ExeFile = TempDir.GetFile(CsFile.GetNameWithoutExtension() + ".exe");

            Scripts.ForEach(t=>t.Text = FixScript(t.Text));
            Compile();

            Execute();
            TempDir.Delete(true);
        }

        private void Execute()
        {
            var p2 = Process.Start(new ProcessStartInfo { FileName = ExeFile.FullName, UseShellExecute = false });
            p2.WaitForExit();
        }

        private void Compile()
        {
            var files = Scripts.Select(t =>
            {
                var file2 = TempDir.GetFile(t.File.Name);
                file2.WriteAllText(t.Text);
                return file2;
            });
            var builder = new ToolArgsBuilder { SwitchSeparatorString = ":" }
                .AddOption("/out", ExeFile.FullName)
                .AddOption("/reference", @"C:\github\corex\src\corex\bin\corex.dll")
                ;
            files.ForEach(t => builder.AddCommand(t.FullName));
            var args = builder.ToString();
            var csc = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            var p = Process.Start(new ProcessStartInfo { FileName = csc, UseShellExecute = false, Arguments = args });//"/out:" + Quote(ExeFile.FullName) + " " + files.Select(t => Quote(t.FullName)).StringJoin(",") }
            p.WaitForExit();
        }
        static string Quote(string s)
        {
            return "\"" + s + "\"";
        }
        List<Script> Scripts = new List<Script>();

        string FixScript(string code)
        {
            var prefix = new StringWriter();
            var suffix = new StringWriter();
            var firstLine = code.Lines().FirstOrDefault();

            var addUsings = false;
            var addClass = false;
            var addMain = false;

            if (firstLine.StartsWith("using"))
            {
            }
            else if (new[] { "class ","namespace " }.FirstOrDefault(t => firstLine.Contains(t)) != null)
            {
                addUsings = true;
            }
            else if (new[] { "void ", "int ", "static ", "public ", "private ", "protected ", "internal " }.FirstOrDefault(t => code.StartsWith(t)) != null)
            {
                addUsings = true;
                addClass = true;
            }
            else
            {
                addUsings = true;
                addClass = true;
                addMain = true;
            }

            if(addUsings)
            {
                prefix.WriteLine("using Corex.Helpers;");
                prefix.WriteLine("using System;");
                prefix.WriteLine("using System.Collections.Generic;");
                prefix.WriteLine("using System.Diagnostics;");
                prefix.WriteLine("using System.IO;");
                prefix.WriteLine("using System.Linq;");
                prefix.WriteLine("using System.Text;");
                prefix.WriteLine("using System.Threading.Tasks;");

            }
            if(addClass)
            {
                prefix.WriteLine("public class MyScript {");
                suffix.WriteLine("}");
            }
            if(addMain)
            {
                prefix.WriteLine("public static void Main(string[] args) {");
                suffix.WriteLine("}");
            }
            prefix.Flush();
            suffix.Flush();
            var p1 = prefix.GetStringBuilder().ToString();
            var p2 = suffix.GetStringBuilder().ToString();
            prefix.Dispose();
            suffix.Dispose();
            return p1 + code + p2;
        }

        public string[] Args { get; set; }

        public FileInfo CsFile { get; set; }

        public DirectoryInfo TempDir { get; set; }

        public FileInfo ExeFile { get; set; }
    }

    class Script
    {
        public string Text { get; set; }
        public FileInfo File { get; set; }
    }
}
