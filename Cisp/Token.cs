using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cisp
{
    enum TokenType
    {
        Number,
        String,
        List,
        Name
    }
    struct TValue
    {
        public double NValue;
        public string SValue;
        public Queue Children;
    };
    struct Token
    {
        public TokenType Type;
        TValue tvalue;
        public object Value 
        {
            get
            {
                switch (Type)
                {
                    case TokenType.List:
                        return tvalue.Children;
                    case TokenType.Name:
                    case TokenType.String:
                        return tvalue.SValue;
                    case TokenType.Number:
                        return tvalue.NValue;
                    default:
                        throw new TException("Unknown type token");
                }
            }
            set
            {
                switch (Type)
                {
                    case TokenType.List:
                        tvalue.Children = (Queue)value;
                        break;
                    case TokenType.Name:
                    case TokenType.String:
                        tvalue.SValue = value.ToString();
                        break;
                    case TokenType.Number:
                        tvalue.NValue = (double)value;
                        break;
                    default:
                        throw new TException("Unknown type token");
                }
            }
        }
    };
}