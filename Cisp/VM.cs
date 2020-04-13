using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cisp
{
    enum CMD
    {
        ADD, MIN, MUL, DIV, MOD, POW,
        DUP, LD, ST, LDC, JLT, JLE, JGT, JGE, JEQ, JNE, JMP,
        PRINT
    }
    struct Command
    {
        public CMD Name;
        public float[] Argument;
        public int Index;
    }
    class VM
    {
        Stack VStack = new Stack();
        ArrayList Memory = new ArrayList();
        int CommandLength;
        int Index = 0;
        Command[] Commands;
        public VM(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Commands = new Command[128];
            foreach (var item in lines)
            {
                string[] split = item.Split(' ');
                //idx name arg1 arg2 arg3...
                int idx;
                int.TryParse(split[0], out idx);
                Command cmd = new Command();
                cmd.Index = idx;
                try
                {
                    cmd.Name = (CMD)Enum.Parse(typeof(CMD), split[1]);
                }
                catch
                {
                    Console.WriteLine("ERROR COMMAND " + split[1]);
                }
                float[] args = new float[split.Length - 2];
                for (int i = 2; i < split.Length; i++)
                {
                    float.TryParse(split[i], out args[i-2]);
                }
                cmd.Argument = args;
                Commands[idx] = cmd;
            }
            CommandLength = Commands.Max(t => t.Index);
        }
        float[] GetArgument()
        {
            return Commands[Index].Argument;
        }
        CMD GetName()
        {
            return Commands[Index].Name;
        }
        public void Run()
        {
            try
            {
                while (Index <= CommandLength)
                {
                    Exec();
                    Index++;
                }
            }
            catch
            {
                Console.WriteLine("Something wrong with Command: {0} Index: {1}", GetName().ToString(), Index);
            }
        }
        public void Exec()
        {
            CMD name = GetName();
            float[] arg = GetArgument();
            float a1, a2;
            float adr, val;
            switch (name)
            {
                case CMD.ADD:
                    a1 = (float)VStack.Pop();
                    a2 = (float)VStack.Pop();
                    VStack.Push(a1 + a2);
                    break;
                case CMD.MIN:
                    a1 = (float)VStack.Pop();
                    a2 = (float)VStack.Pop();
                    VStack.Push(a1 - a2);
                    break;
                case CMD.MUL:
                    a1 = (float)VStack.Pop();
                    a2 = (float)VStack.Pop();
                    VStack.Push(a1 * a2);
                    break;
                case CMD.DIV:
                    a1 = (float)VStack.Pop();
                    a2 = (float)VStack.Pop();
                    VStack.Push(a1 / a2);
                    break;
                case CMD.PRINT:
                    Console.WriteLine(VStack.Pop());
                    break;
                case CMD.DUP:
                    VStack.Push(VStack.Peek());
                    break;
                case CMD.LD:
                    adr = (float)VStack.Pop();
                    if (adr >= 0f)
                    {
                        VStack.Push(Memory[(int)adr]);
                    }
                    break;
                case CMD.ST:
                    val = (float)VStack.Pop();
                    adr = (float)VStack.Pop();
                    if (adr >= 0f)
                    {
                        Memory[(int)adr] = val;
                    }
                    break;
                case CMD.LDC:
                    for (int i = 0; i < arg.Length; i++)
                    {
                        VStack.Push(arg[arg.Length-1-i]);
                    }
                    break;
                case CMD.JLT:
                    if ((float)VStack.Pop() < 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JLE:
                    if ((float)VStack.Pop() <= 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JGT:
                    if ((float)VStack.Pop() > 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JGE:
                    if ((float)VStack.Pop() >= 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JEQ:
                    if ((float)VStack.Pop() == 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JNE:
                    if ((float)VStack.Pop() != 0f)
                    {
                        Index = (int)arg[0];
                    }
                    break;
                case CMD.JMP:
                    Index = (int)arg[0];
                    break;
                default:
                    Console.WriteLine("Unknown command: {0}", name.ToString());
                    break;
            }
        }
    }
}
