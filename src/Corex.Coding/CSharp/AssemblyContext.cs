using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.CodingTools.CSharp
{
    public class AssemblyContext
    {
        public List<Assembly> Assemblies { get; set; }

        public Class FindClassByName(string name, Assembly asmToCreateIfMissing = null)
        {
            if (name.IsNullOrEmpty())
                return null;
            foreach (var asm in Assemblies)
            {
                var x = FindClassByName(asm, name, false);
                if (x != null)
                {
                    if (asmToCreateIfMissing != null && asm != asmToCreateIfMissing)
                    {
                        asmToCreateIfMissing.Classes.Add(x);
                        asm.Classes.Remove(x);
                    }
                    return x;
                }
            }
            if (asmToCreateIfMissing == null)
                asmToCreateIfMissing = Assemblies.First();
            var ce = new Class { Name = name };
            asmToCreateIfMissing.Classes.Add(ce);
            return ce;
        }
        public Class FindClassByName(Assembly asm, string name, bool create)
        {
            var ce = asm.Classes.Where(t => t.Name.EqualsIgnoreCase(name)).FirstOrDefault();
            if (ce != null)
                return ce;
            if (create)
            {
                ce = new Class { Name = name };
                asm.Classes.Add(ce);
            }
            return ce;
        }
        public Class MakeGenericClassByNames(string name, string[] genericArgs)
        {
            var list = new List<Class>();
            foreach (var arg in genericArgs)
            {
                list.Add(FindClassByName(arg));
            }
            var ce = FindClassByName(name + "`" + genericArgs.Length);
            return ce.MakeGenericClass(list.ToArray());
        }

    }
}
