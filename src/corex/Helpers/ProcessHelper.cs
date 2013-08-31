using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.Helpers
{
    public static class ProcessHelper
    {
        public static Process Current
        {
            get
            {
                return Process.GetCurrentProcess();
            }
        }

        public static FileInfo CurrentProcessFile { get { return Current.MainModule.FileName.ToFileInfo(); } }
    }
}
