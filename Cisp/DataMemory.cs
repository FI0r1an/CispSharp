using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CispVM
{
    struct DataMemory
    {
        private readonly Data[] Memory;
        private int Index;
        public DataMemory(int len)
        {
            Memory = new Data[len];
            Index = 0;
        }
        public void Add(Data data)
        {
            if (Index > Memory.Length - 1) throw new IndexOutOfRangeException();
            Memory[Index++] = data;
        }
        public Data Remove()
        {
            if (Index < 0) throw new IndexOutOfRangeException();
            var v = Memory[Index];
            Memory[Index] = null;
            Index--;
            return v;
        }
        public Data Remove(int idx)
        {
            if (idx < 0 || idx > (Memory.Length - 1)) throw new IndexOutOfRangeException();
            var v = Memory[idx];
            Memory[idx] = null;
            return v;
        }
        public Data this[int idx]
        {
            get
            {
                return (idx >= Memory.Length) ? (Data)0 : Memory[idx];
            }
            set
            {
                Memory[idx] = value;
            }
        }
    }
}
