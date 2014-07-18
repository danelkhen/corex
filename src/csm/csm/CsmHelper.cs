using Corex.Helpers;
using Corex.IO.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace csm
{
    public static class CsmExtensions
    {
        public static void ExecuteAsCsmFile(this string filename)
        {
            var csm = new CsmHelper { CsFile = filename.ToFileInfo() };
            csm.Run();
        }
        public static void ExecuteAsCsmScript(this string script)
        {
            script.SaveAsTempFile().ExecuteAsCsmFile();
        }
    }

    public class CsmHelper
    {

        public List<string> OneLines { get; set; }

        public void Run()
        {
            LoadCsFile();
            CreateTempDir();
            AllocateExeFile();
            FixScripts();
            Compile();
            LoadExeIntoCurrentProcess();
            //Running locally, can't delete loaded assembly TempDir.Delete(true);
        }

        private void FixScripts()
        {
            Scripts.ForEach(t => t.Text = FixScript(t.Text));
        }

        private void AllocateExeFile()
        {
            ExeFile = TempDir.GetFile(Path.GetTempFileName().ToFileInfo().Name);
            if(CsFile!=null)
                ExeFile = TempDir.GetFile(CsFile.GetNameWithoutExtension() + ".exe");
        }

        private void CreateTempDir()
        {
            var tempFile = Path.GetTempFileName().ToFileInfo();
            TempDir = Environment.ExpandEnvironmentVariables("%temp%").ToDirectoryInfo().GetDirectory(tempFile.GetNameWithoutExtension());
            TempDir.VerifyExists();
        }

        private void LoadCsFile()
        {
            if (OneLines != null)
                OneLines.ForEach(t => Scripts.Add(new Script { File = t.SaveAsTempFile().ToFileInfo(), Text = t }));
            if(CsFile!=null)
                Scripts.Add(new Script { File = CsFile, Text = CsFile.ReadAllText() });
        }
        Assembly LoadedExe;
        private void LoadExeIntoCurrentProcess()
        {
            LoadedExe = Assembly.LoadFrom(ExeFile.FullName);
            if (LoadedExe.EntryPoint != null)
            {
                try
                {
                    LoadedExe.EntryPoint.Invoke(null, new object[] { new string[0] });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }
            }
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
                .AddOption("/reference", @"C:\github\corex\src\csm\csm\bin\Debug\csm.exe")
                ;
            files.ForEach(t => builder.AddCommand(t.FullName));
            var args = builder.ToString();
            var csc = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";

            var output = new StringBuilder();
            var p = csc.ToProcess(args).Execute(output);
            if (p.ExitCode != 0)
            {
                throw new Exception("Script compilation failed for " + Scripts.Select(t => t.File.GetOriginalPath()).StringJoin(",") + "\n" + output.ToString());
            }
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
            else if (new[] { "class ", "namespace " }.FirstOrDefault(t => firstLine.Contains(t)) != null)
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

            if (addUsings)
            {
                prefix.WriteLine("using Corex.Helpers;");
                prefix.WriteLine("using csm;");
                prefix.WriteLine("using System;");
                prefix.WriteLine("using System.Collections.Generic;");
                prefix.WriteLine("using System.Diagnostics;");
                prefix.WriteLine("using System.IO;");
                prefix.WriteLine("using System.Linq;");
                prefix.WriteLine("using System.Text;");
                prefix.WriteLine("using System.Threading.Tasks;");

            }
            if (addClass)
            {
                prefix.WriteLine("public class MyScript {");
                suffix.WriteLine("}");
            }
            if (addMain)
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

        public FileInfo CsFile { get; set; }

        public DirectoryInfo TempDir { get; set; }

        public FileInfo ExeFile { get; set; }

    }
}
