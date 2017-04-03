using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PandaScraper
{
    public static class DateExtensions
    {
        public static string ToTicksFromMaxString(this DateTime dt)
        {
            return $"{DateTime.MaxValue.Ticks - dt.ToUniversalTime().Ticks:D19}";
        }

        public static DateTime ReverseToTicks(this string ticksString)
        {
            var numeric = long.Parse(ticksString);

            return new DateTime(DateTime.MaxValue.Ticks - numeric);
        }
    }
}