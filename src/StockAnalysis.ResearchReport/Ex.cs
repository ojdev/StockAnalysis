using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalysis.ResearchReport
{
    public static class Ex
    {
        public static string NotNullValue(this string val)
        {
            return string.IsNullOrWhiteSpace(val) ? " " : val;
        }
    }
}
