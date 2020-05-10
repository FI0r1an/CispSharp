using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CispVM
{
    enum OPCODE
    {
        HALT,
        LOAD,
        STORE,
        DUP,
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,
        SHR,
        SHL,
        NOT,
        XOR,
        OR,
        AND,
        INC,
        DEC,
        PUSH,
        POP,
        LT,
        GT,
        LE,
        GE,
        EQ,
        NE,
        JTRUE,
        JFALSE,
        JMP,
        CALL,
        IN,
        OUT
        /*LOADR,
        INCR,
        DECR,
        PUSHR,
        RET,
        PREG,
        PTOP*/
    }
    struct Instruction
    {
        public OPCODE Opcode;
        public int A;
        public int B;
        public int C;
        public Instruction(OPCODE opcode, int? a, int? b, int? c)
        {
            Opcode = opcode;
            A = a ?? 0;
            B = b ?? 0;
            C = c ?? 0;
        }
    }
}
