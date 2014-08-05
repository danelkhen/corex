using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Csm
{
    public static class CsmExtensions
    {
        public static void ExecuteAsCsmFile(this string filename)
        {
            CsmRuntime.VerifyInit().Run(new CsmScript { File = filename.ToFileInfo() });
        }
        public static void ExecuteAsCsmScript(this string script)
        {
            CsmRuntime.VerifyInit().Run(new CsmScript { Code = script });
        }
    }
}
