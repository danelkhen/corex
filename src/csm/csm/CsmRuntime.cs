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

    public class CsmRuntime
    {
        public List<CsmCompilation> Compilations { get; set; }

        public DirectoryInfo TempDir { get; set; }

        public CsmRuntime()
        {
            Compilations = new List<CsmCompilation>();
        }

        public static CsmRuntime VerifyInit()
        {
            if (Current == null)
                Current = new CsmRuntime();
            return Current;
        }
        
        public static CsmRuntime Current { get; set; }

        public void Run()
        {
            VerifyTempDir();
            Compilations.ForEach(t => t.Run());
        }

        void VerifyTempDir()
        {
            if (TempDir == null)
                TempDir = ProcessHelper.CurrentProcessFile.GetParent().GetDirectory("temp").GetDirectory(DateTime.Now.ToFileTime().ToString());
            Compilations.Where(t => t.TempDir == null).ForEach(t => t.TempDir = TempDir);
            TempDir.VerifyExists();
        }

        public void Run(CsmScript cs)
        {
            Run(new CsmCompilation { Scripts = { cs } });
        }
        public void Run(CsmCompilation cc)
        {
            Compilations.Add(cc);
            VerifyTempDir();
            cc.Run();
        }
    }


}
