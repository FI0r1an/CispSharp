using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CispVM
{
    enum STEP
    {
        NEXT,
        ERROR,
        JUMP,
        HALT
    }
    class VM
    {
        private readonly Stack<Data> DStack;
        private DataMemory DMem;
        //private DataMemory BMem;
        public DataMemory CMem;
        private readonly Instruction[] IMem;

        private const int MEM_SIZE = 128;
        private const int STK_SIZE = 16;

        private STEP Result;

        private int IP;
        public void Assert(bool b)
        {
            if (!b) throw new Exception($"ERROR: At Index{IP}");
        }
        public void Assert(bool b, string msg)
        {
            if (!b) throw new Exception($"{msg}: At Index{IP}");
        }
        public VM(Instruction[] imem)
        {
            DStack = new Stack<Data>(STK_SIZE); //data stack
            //BMem = new DataMemory(STK_SIZE); //buffer memory
            DMem = new DataMemory(MEM_SIZE); //data memory
            CMem = new DataMemory(MEM_SIZE); //const memory
            IMem = imem; //instruction memory
            IP = 0; //instrucion pointer
        }
        private Data GetDataFromMemory(int idx)
        {
            return (idx + 1 <= MEM_SIZE) ? DMem[idx] : CMem[idx - MEM_SIZE];
        }
        private Data Pop()
        {
            Assert(DStack.Count > 0, "Out of range");
            return DStack.Pop();
        }
        private void Push(Data data)
        {
            DStack.Push(data);
        }
        private void SetDataFromMemory(int idx, Data data)
        {
            Assert(idx + 1 <= MEM_SIZE, "Out of range");
            DMem[idx] = data;
        }
        private void BinaryOperation(OPCODE opcode)
        {
            Data r = 0;
            Data a2 = Pop(), a1 = Pop();
            switch (opcode)
            {
                case OPCODE.ADD:
                    r = (double)a1 + a2;
                    break;
                case OPCODE.SUB:
                    r = (double)a1 - a2;
                    break;
                case OPCODE.MUL:
                    r = (double)a1 * a2;
                    break;
                case OPCODE.DIV:
                    if (a2 == 0f) Result = STEP.ERROR;
                    r = (double)a1 / a2;
                    break;
                case OPCODE.MOD:
                    r = (double)a1 % a2;
                    break;
                case OPCODE.SHR:
                    r = a1 >> a2;
                    break;
                case OPCODE.SHL:
                    r = a1 << a2;
                    break;
            }
            Push(r);
        }
        private void UnaryOperation(OPCODE opcode)
        {
            Data r = 0;
            Data a1 = Pop();
            switch (opcode)
            {
                case OPCODE.NOT:
                    r = !a1;
                    break;
                case OPCODE.INC:
                    r = a1 + 1f;
                    break;
                case OPCODE.DEC:
                    r = a1 - 1f;
                    break;
            }
            Push(r);
        }
        private void BinaryBoolOperation(OPCODE opcode)
        {
            Data r = false;
            Data a2 = Pop(), a1 = Pop();
            switch (opcode)
            {
                case OPCODE.LT:
                    r = a1 < a2;
                    break;
                case OPCODE.GT:
                    r = a1 > a2;
                    break;
                case OPCODE.LE:
                    r = a1 <= a2;
                    break;
                case OPCODE.GE:
                    r = a1 >= a2;
                    break;
                case OPCODE.EQ:
                    r = ((double)a1 == a2) || ((bool)a1 == a2) || ((string)a1 == a2);
                    break;
                case OPCODE.NE:
                    r = ((double)a1 != a2) || ((bool)a1 != a2) || ((string)a1 != a2);
                    break;
                case OPCODE.OR:
                    r = a1 || a2;
                    break;
                case OPCODE.XOR:
                    r = (bool)a1 != a2;
                    break;
                case OPCODE.AND:
                    r = a1 && a2;
                    break;
            }
            Push(r);
        }
        private void InstLoad(int a)
        {
            Push(GetDataFromMemory(a));
        }
        private void InstStore(int a)
        {
            SetDataFromMemory(a, Pop());
        }
        private void InstDup()
        {
            if (DStack.Count <= 0) Result = STEP.ERROR;
            Push(DStack.Peek());
        }
        private void InstPush(int a)
        {
            Push(a);
        }
        private void InstPop()
        {
            Pop();
        }
        private void InstJump(int a)
        {
            IP = a;
            Result = STEP.JUMP;
        }
        private void InstJTrue(int a)
        {
            if (Pop()) IP = a;
            Result = STEP.JUMP;
        }
        private void InstJFalse(int a)
        {
            if (!Pop()) IP = a;
            Result = STEP.JUMP;
        }
        public void StepVM(Instruction current)
        {
            var opcode = current.Opcode;
            int a = current.A;
            Result = STEP.NEXT;
            switch (opcode)
            {
                case OPCODE.HALT:
                    Result = STEP.HALT; break;
                case OPCODE.LOAD:
                    InstLoad(a); break;
                case OPCODE.STORE:
                    InstStore(a); break;
                case OPCODE.DUP:
                    InstDup(); break;
                case OPCODE.ADD:
                case OPCODE.SUB:
                case OPCODE.MUL:
                case OPCODE.DIV:
                case OPCODE.MOD:
                case OPCODE.SHR:
                case OPCODE.SHL:
                    BinaryOperation(opcode); break;
                case OPCODE.NOT:
                case OPCODE.INC:
                case OPCODE.DEC:
                    UnaryOperation(opcode); break;
                case OPCODE.PUSH:
                    InstPush(a); break;
                case OPCODE.POP:
                    InstPop(); break;
                case OPCODE.LT:
                case OPCODE.GT:
                case OPCODE.LE:
                case OPCODE.GE:
                case OPCODE.EQ:
                case OPCODE.NE:
                case OPCODE.OR:
                case OPCODE.XOR:
                case OPCODE.AND:
                    BinaryBoolOperation(opcode); break;
                case OPCODE.JTRUE:
                    InstJTrue(a); break;
                case OPCODE.JFALSE:
                    InstJFalse(a); break;
                case OPCODE.JMP:
                    InstJump(a); break;
                case OPCODE.CALL:
                    break;
                case OPCODE.IN:
                    Console.Write("Input: ");
                    Push(double.Parse(Console.ReadLine()));
                    break;
                case OPCODE.OUT:
                    Console.WriteLine($"Output: {Pop()}");
                    break;
                default:
                    Result = STEP.ERROR; break;
            }
        }
        public void Execute()
        {
            var len = IMem.Length;
            for (; ; )
            {
                if (IP >= len) break;
                StepVM(IMem[IP]);
                switch (Result)
                {
                    case STEP.ERROR:
                        throw new Exception("Error");
                    case STEP.NEXT:
                        IP++;
                        break;
                    case STEP.HALT:
                        return;
                }
            }
        }
    }
}
