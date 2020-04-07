using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cisp
{
    class TException : ApplicationException
    {
        public TException() { }
        public TException(string msg) : base(msg)
        {

        }
    }
}
