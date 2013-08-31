using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corex.CodingTools.CSharp
{
    public static class Extensions
    {

        public static T Clone<T>(this T obj) where T : Member
        {
            return obj.Clone2() as T;
        }
    }
}
