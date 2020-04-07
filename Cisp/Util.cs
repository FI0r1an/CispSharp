using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cisp
{
    static class Util
    {
        public static bool IsLine(string str)
        {
            return (str[0] == '\r' || str[0] == '\n');
        }
        public static bool IsQuote(string str)
        {
            return (str[0] == '\'' || str[0] == '\"');
        }
        public static bool IsAlpha(string str)
        {
            return ('a' <= str[0] && str[0] <= '9') || ('A' <= str[0] && str[0] <= 'Z');
        }
        public static bool IsDigit(string str)
        {
            return ('0' <= str[0] && str[0] <= '9');
        }
        public static bool IsSpace(string str)
        {
            return ('\t' == str[0] || ' ' == str[0]);
        }
        public static bool IsWhitespace(string str)
        {
            return IsSpace(str) || IsLine(str);
        }
    }
}
