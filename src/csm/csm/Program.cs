using Corex.Helpers;
using Corex.IO.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Csm
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
                Console.Error.WriteLine(e.Message);
                return -1;
            }
        }

        private void Run()
        {
            CsmRuntime.VerifyInit();
            var args = new ToolArgsParser().Parse(Args).ToList();
            var csFiles = args.Where(t => t.Name == null && t.Switch == null && t.Value != null).Select(t=>t.Value).ToList();
            var oneLines = args.Where(t => t.Name == "e").Select(t => t.Value).ToList();

            var csFilename = csFiles.FirstOrDefault();
            var Csm = new CsmRuntime { Compilations = { new CsmCompilation { Scripts = oneLines.Select(t => new CsmScript { Code = t }).ToList() } } };
            if (csFilename != null)
            {
                var CsFile = csFilename.ToFileInfo();
                if (!CsFile.Exists && CsFile.Extension != ".cs")
                    CsFile = (CsFile.FullName + ".cs").ToFileInfo();
                if (!CsFile.Exists)
                    throw new Exception("File not found " + csFilename);
                Csm.Compilations[0].Scripts.Add(new CsmScript { File = CsFile });
            }
            if(Csm.Compilations[0].Scripts.IsEmpty())
            {
                while(true)
                {
                    var line = Console.ReadLine();
                    if (line.IsNullOrEmpty())
                        continue;
                    try
                    {
                        line.ExecuteAsCsmScript();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            Csm.Run();
        }

        public string[] Args { get; set; }

    }

}
