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
            var args = new ToolArgsParser().Parse(Args).ToList();
            var csFiles = args.Where(t => t.Name == null && t.Switch == null && t.Value != null).Select(t=>t.Value).ToList();
            var oneLines = args.Where(t => t.Name == "e").Select(t => t.Value).ToList();

            var csFilename = csFiles.FirstOrDefault();
            var Csm = new CsmHelper { OneLines = oneLines };
            if (csFilename != null)
            {
                var CsFile = csFilename.ToFileInfo();
                if (!CsFile.Exists && CsFile.Extension != ".cs")
                    CsFile = (CsFile.FullName + ".cs").ToFileInfo();
                if (!CsFile.Exists)
                    throw new Exception("File not found " + csFilename);
                Csm.CsFile = CsFile;
            }
            if(Csm.CsFile==null && Csm.OneLines.IsNullOrEmpty())
            {
                while(true)
                {
                    var line = Console.ReadLine();
                }
            }
            Csm.Run();
        }

        public string[] Args { get; set; }

    }

}
