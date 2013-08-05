using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corex.Helpers
{
    public class ParseHelper
    {
        public static int? TryInt(string s)
        {
            int x;
            if (int.TryParse(s, out x))
                return x;
            return null;
        }
        public static bool? TryBoolean(string s)
        {
            bool x;
            if (bool.TryParse(s, out x))
                return x;
            return null;
        }

        public static long? TryLong(string s)
        {
            long x;
            if (long.TryParse(s, out x))
                return x;
            return null;
        }
    }
}
