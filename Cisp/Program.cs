using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CispVM
{
    class Program
    {
        static Instruction[] Parse(string path)
        {
            var line = File.ReadAllLines(path);
            var result = new Instruction[line.Length];
            for (int i = 0;i < result.Length;i++)
            {
                Instruction inst;
                var split = line[i].Split(' ');

                inst = new Instruction((OPCODE)Enum.Parse(typeof(OPCODE), split[0]), int.Parse((split.Length < 2) ? "0" : split[1]), null, null);
                result[i] = inst;
            }
            return result;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            VM vm = new VM(Parse("test.txt"));
            vm.CMem.Add("Hello World");
            vm.CMem.Add("Made by Cluck");
            vm.Execute();
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.ReadLine();
        }
    }
}
