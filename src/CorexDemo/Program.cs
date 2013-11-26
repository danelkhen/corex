using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corex.Helpers;
using Corex.IO;
using Corex.IO.Tools;

namespace CorexDemo
{
    class Program
    {

        static void foo()
        {
            FsPath xx = "C:\\";
            var xxx = xx + "abc" + "def\\";

        }

        static void foo2()
        {
            //WindowsServiceHelper.CreateService(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine(FrameworkVersion.HasVersionkOrBetter(new FrameworkVersion(new Version("4.0"), FrameworkVariant.Client)));
            Console.Read();
        }
        static void Main(string[] args)
        {
            foo2();
        }
        void foo3()
        {
            var args2 = new string[] { "/Name:Hello" };
            var tool = new ToolArgsInfo<Program>();
            var x = tool.Parse(args2);
            Console.WriteLine(tool.Generate(x));
            Console.WriteLine(tool.GenerateHelp());

        }
        public string Name { get; set; }
    }

    class MyAppArgs
    {
        public string Command { get; set; }
        public string Name { get; set; }
    }
}
