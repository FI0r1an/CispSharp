using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CispVM
{
    enum DataType
    {
        Nil,
        Boolean,
        Double,
        String
    }
    struct Data
    {
        public const double TRUE = 1f;
        public const double FALSE = 0f;
        public DataType Type;
        public double Double;
        public string String;
        public static implicit operator Data(double v)
        {
            return new Data(DataType.Double, v);
        }
        public static implicit operator Data(bool v)
        {
            return new Data(DataType.Boolean, v ? TRUE : FALSE);
        }
        public static implicit operator Data(string v)
        {
            return new Data(DataType.String, v);
        }
        public static implicit operator double(Data v)
        {
            return v.Double;
        }
        public static implicit operator int(Data v)
        {
            return (int)v.Double;
        }
        public static implicit operator bool(Data v)
        {
            return (v.Double == TRUE) ? true : false;
        }
        public static implicit operator string(Data v)
        {
            return v.String;
        }
        public override string ToString() => $"Type: {Type}, Double: {Double}, String: {String}";
        public Data(DataType dt, object v)
        {
            Double = 0;
            String = "";
            Type = dt;
            switch (dt)
            {
                case DataType.String:
                    String = v.ToString();
                    return;
                case DataType.Boolean:
                case DataType.Double:
                    Double = (double)v;
                    break;
            }
        }
    }
}
