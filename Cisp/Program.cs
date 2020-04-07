using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cisp
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserState ps = new ParserState("(\r\n\t 1 2 (1 2 3))");
            Parser p = new Parser(ps);
            var tkv = (Queue)(p.ReadToken().Value);
            foreach (Token item in tkv)
            {
                Console.WriteLine(item.Type.ToString());
                Console.WriteLine(item.Value.ToString());
            }
            Console.WriteLine("=======End=======");
            Console.ReadLine();
        }
    }
}
