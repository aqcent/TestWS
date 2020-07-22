using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Extensions
{
    public static class StringExtensions
    {
        public static bool CompareIgnoreCase(this string value, string compareText, StringComparison StringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return value.IndexOf(compareText, StringComparison) >= 0;
        }
    }
}