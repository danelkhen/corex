using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Csm
{
    public class CsmScript
    {
        public string Code { get; set; }
        public string FinalCode { get; set; }
        public FileInfo File { get; set; }
    }
}
