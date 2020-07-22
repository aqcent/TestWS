using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Extensions
{
    public static class IntExtensions
    {
        public static string ToDuration(this int value)
        {
            var hours = Math.Truncate(value / 60f);
            var minute = value % 60;
            return $"{hours}h {minute}m";
        }
    }
}