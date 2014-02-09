using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSymbolPackage
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "hello";
            s = Yalib.StrHelper.EnsureEndWith(s, ".NET");
        }
    }
}
