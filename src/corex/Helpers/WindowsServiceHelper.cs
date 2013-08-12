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
        public static void CreateService(string binPath)
        {
            var serviceName = binPath.ToFsPath().NameWithoutExtension;
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
        public static void DeleteService(string binPath)
        {
            var serviceName = binPath.ToFsPath().NameWithoutExtension;
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
    }


}
