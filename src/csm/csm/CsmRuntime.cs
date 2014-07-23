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

namespace Csm
{
    public static class CsmExtensions
    {
        public static void ExecuteAsCsmFile(this string filename)
        {
            var csm = new CsmRuntime { Scripts = { new CsmScript { File = filename.ToFileInfo() } } };
            csm.Run();
        }
        public static void ExecuteAsCsmScript(this string script)
        {
            script.SaveAsTempFile().ExecuteAsCsmFile();
        }
    }

    public class CsmRuntime
    {
        public DirectoryInfo TempDir { get; set; }
        public FileInfo ExeFile { get; set; }
        public Assembly LoadedExe { get; set; }
        public string MainScriptName { get; set; }
        public List<CsmScript> Scripts { get; set; }

        public CsmRuntime()
        {
            Scripts = new List<CsmScript>();
        }
        
        public static void Init()
        {
            if (Current != null)
                return;
            Current = new CsmRuntime();
        }
        
        public static CsmRuntime Current { get; set; }

        public void Run()
        {
            VerifyMainScriptName();
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

        void VerifyMainScriptName()
        {
            if (MainScriptName.IsNotNullOrEmpty())
                return;
            var csFile = Scripts.FirstOrDefault(t => t.File != null);
            if (csFile != null)
            {
                MainScriptName = csFile.File.GetNameWithoutExtension();
            }
            else
            {
                MainScriptName = Path.GetTempFileName().ToFileInfo().GetNameWithoutExtension();
            }

        }
        void AllocateExeFile()
        {
            VerifyMainScriptName();
            ExeFile = TempDir.GetFile(MainScriptName + ".exe");
        }

        void CreateTempDir()
        {
            TempDir = ProcessHelper.CurrentProcessFile.GetParent().GetDirectory("temp").GetDirectory(DateTime.Now.ToFileTime().ToString()).VerifyExists();
        }

        void LoadCsFile()
        {
            Scripts.ForEach(t =>
                {
                    if (t.File != null && t.Text == null && t.File.Exists)
                        t.Text = File.ReadAllText(t.File.FullName);
                });
        }
        void LoadExeIntoCurrentProcess()
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

        void Compile()
        {
            var files = Scripts.Select(t =>
            {
                FileInfo file2;
                if (t.File != null)
                    file2 = TempDir.GetFile(t.File.Name);
                else
                    file2 = TempDir.GetFile(Path.GetTempFileName().ToFileInfo().Name);
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
                throw new Exception("Script compilation failed for " + Scripts.Select(ScriptToErrorText).StringJoin(",") + "\n" + output.ToString());
            }
        }
        static string ScriptToErrorText(CsmScript f)
        {
            if (f.File != null)
                return f.File.GetOriginalPath();
            return "Dynamic Script: " + f.Text;
        }
        static string Quote(string s)
        {
            return "\"" + s + "\"";
        }

        string FixScript(string code)
        {
            var prefix = new StringWriter();
            var suffix = new StringWriter();
            var firstLine = code.Lines().FirstOrDefault();

            var addUsings = false;
            var addClass = false;
            var addMain = false;
            var addNamespace = false;
            if (firstLine.StartsWith("using"))
            {
            }
            else if (new[] { "namespace " }.FirstOrDefault(t => firstLine.Contains(t)) != null)
            {
                addUsings = true;
            }
            else if (new[] { "class " }.FirstOrDefault(t => firstLine.Contains(t)) != null)
            {
                addUsings = true;
                addNamespace = true;
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
                prefix.WriteLine("using Csm;");
                prefix.WriteLine("using System;");
                prefix.WriteLine("using System.Collections.Generic;");
                prefix.WriteLine("using System.Diagnostics;");
                prefix.WriteLine("using System.IO;");
                prefix.WriteLine("using System.Linq;");
                prefix.WriteLine("using System.Text;");
                prefix.WriteLine("using System.Threading.Tasks;");

            }
            if (addNamespace)
            {
                prefix.WriteLine("namespace Csm.Scripts {");
                suffix.WriteLine("}");
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


    }


    //public class CsmCompilation
    //{
    //    public DirectoryInfo TempDir { get; set; }
    //    public FileInfo ExeFile { get; set; }
    //    public Assembly LoadedExe { get; set; }
    //    public string MainScriptName { get; set; }
    //    public List<CsmScript> Scripts { get; set; }

    //}
}
