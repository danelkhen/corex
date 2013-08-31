using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptParser.Parser
{
    public static class Extensions
    {
        public static StringSelection Select(this string s, int index, int length)
        {
            return s.Locate(index).Select(length);
        }
        public static StringLocation Locate(this string s, int index)
        {
            return new StringLocation(s, index);
        }

        public static int CountRepeatingByType(this IEnumerable<Token> list, params TokenType[] types)
        {
            return list.CountRepeating(t => t.Is(types));
        }

        public static int CountRepeating(this IEnumerable<Token> list, Func<Token, bool> predicate)
        {
            var count = 0;
            foreach (var next in list)
            {
                if (!predicate(next))
                    break;
                count++;
            }
            return count;
        }

    }
}
