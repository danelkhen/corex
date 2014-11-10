using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

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


        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable list)
        {
            return new TypedEnumerable<T>(list);
        }

        class TypedEnumerable<T> : IEnumerable<T>
        {
            private IEnumerable Source;
            public TypedEnumerable(IEnumerable source)
            {
                Source = source;
            }
            
            public IEnumerator<T> GetEnumerator()
            {
                foreach(T t in Source)
                {
                    yield return t;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
