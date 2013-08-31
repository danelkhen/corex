using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public static class DiagnosticsExtensions
    {
        public static string MainModuleFilename(this Process p)
        {
            return p.MainModule.FileName;
        }
    }
}
