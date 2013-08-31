using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser;

namespace TypeScriptParserTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new[]
                {
                    "backbone.d.ts",
                    "node.d.ts",
                    "lib.d.ts",
                };
            foreach (var file in files)
            {
                var parser = new TsParser();
                var filename = @"..\\..\\res\\" + file;
                var code = File.ReadAllText(filename);

                var parsed = parser.Parse(code);
            }
        }
    }
}
