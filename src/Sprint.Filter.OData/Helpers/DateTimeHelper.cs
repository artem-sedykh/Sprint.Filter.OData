using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Sprint.Filter.Helpers
{
    internal static class DateTimeHelper
    {
        private static readonly Regex DateTimeRegex = new Regex(@"^(?<Year>\d{4})-(?<Month>\d{2})-(?<Day>\d{2})T(?<Hour>\d{2}):(?<Minute>\d{2})(\:(?<Second>\d{2})(\.(?<Millisecond>\d{1,7}))?)?(?<TZ>(Z|[-+]\d{2}\:\d{2})?)$");

        public static DateTime Parse(string s)
        {
            var match = DateTimeRegex.Match(s);

            if (!match.Success)
                throw new Exception($"Date/time format is invalid at {s}.");

            var value =  DateTime.Parse(s, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            return value;

        }
    }
}
