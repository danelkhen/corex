using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Corex.IO;
using Corex.IO.Tools;

namespace Corex.Helpers
{
    public class WindowsServiceHelper
    {
        static ToolArgsBuilder Build()
        {
            return new ToolArgsBuilder { SwitchSeparatorString = "= " };
        }
        public static void CreateService(string serviceName, string binPath)
        {
            var tool = new Tool
            {
                Filename = "sc.exe",
                Args = Build()
                    .AddCommand("create")
                    .AddCommand(serviceName)
                    .AddOption("binPath", binPath).ToString(),
            };
            tool.Run();
        }
        public static void StartService(string serviceName)
        {
            var tool = new Tool
            {
                Filename = "sc.exe",
                Args = Build()
                    .AddCommand("start")
                    .AddCommand(serviceName)
                    .ToString(),
            };
            tool.Run();
        }
        public static void StopService(string serviceName)
        {
            var tool = new Tool
            {
                Filename = "sc.exe",
                Args = Build()
                    .AddCommand("stop")
                    .AddCommand(serviceName)
                    .ToString(),
            };
            tool.Run();
        }
        public static void DeleteService(string serviceName)
        {
            var tool = new Tool
            {
                Filename = "sc.exe",
                Args = Build()
                    .AddCommand("delete")
                    .AddCommand(serviceName)
                    .ToString(),
            };
            tool.Run();

        }
    }


}
