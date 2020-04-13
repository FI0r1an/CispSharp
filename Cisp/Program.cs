using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cisp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Run a .vcm file: ");
                string file = Console.ReadLine();
                try
                { 
                    VM vm = new VM(file);
                    vm.Run();
                }
                catch
                {
                    Console.WriteLine("\"{0}\" Doesnt exist!", file);
                }
                finally
                {
                    Console.WriteLine("=========END=========");
                }
            }
        }
    }
}
