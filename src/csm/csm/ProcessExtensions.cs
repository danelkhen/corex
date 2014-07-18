using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csm
{
    public static class ProcessExtensions
    {
        public static Process ToProcess(this string filename, string args = null)
        {
            return new Process { StartInfo = new ProcessStartInfo { FileName = filename, Arguments = args } };
        }
        public static Process ToProcess(this FileInfo file, string args = null)
        {
            return file.FullName.ToProcess(args);
        }

        public static string SaveAsTempBatchFile(this string script)
        {
            var file = Path.GetTempFileName() + ".bat";
            File.WriteAllText(file, script);
            return file;
        }
        public static string SaveAsTempFile(this string text)
        {
            var file = Path.GetTempFileName();
            File.WriteAllText(file, text);
            return file;
        }

        public static int ExecuteAsBatchScript(this string script, string args = null)
        {
            return script.SaveAsTempBatchFile().ToProcess().Execute(args);
        }

        public static int Execute(this Process p, string args = null)
        {
            if (args != null)
                p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
            return p.ExitCode;
        }
        public static Process Args(this Process p, string args)
        {
            p.StartInfo.Arguments = args;
            return p;
        }

        public static Process Execute(this Process p, StringBuilder output)
        {
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            if (output != null)
            {
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
            }
            p.Start();
            if (output != null)
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.OutputDataReceived += (s, e) => output.AppendLine(e.Data);
                p.ErrorDataReceived += (s, e) => output.AppendLine(e.Data);
            }
            p.WaitForExit();
            return p;
        }

        public static string ExecuteGetOutput(this Process p)
        {
            var output = new StringBuilder();
            p.Execute(output);
            return output.ToString();
        }

    }


    public class ProcessRun
    {
        public Process Process { get; set; }
        public StringBuilder OutputAndError { get; set; }
    }
}
