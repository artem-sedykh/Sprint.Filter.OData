using System;
using System.Text.RegularExpressions;

namespace Sprint.Filter.Helpers
{
    internal static class DateTimeHelper
    {
        private static readonly Regex DateTimeRegex = new Regex(@"^(?<Year>\d{4})-(?<Month>\d{2})-(?<Day>\d{2})T(?<Hour>\d{2}):(?<Minute>\d{2})(\:(?<Second>\d{2})(\.(?<Millisecond>\d{7}))?)?(?<TZ>(Z|[-+]\d{2}\:\d{2})?)$");

        public static DateTime Parse(string s)
        {
            var match = DateTimeRegex.Match(s);

            if (match.Success)
            {
                var year = int.Parse(match.Groups["Year"].Value);
                var month = int.Parse(match.Groups["Month"].Value);
                var day = int.Parse(match.Groups["Day"].Value);

                var hour = match.Groups["Hour"].Value.Length > 0 ? int.Parse(match.Groups["Hour"].Value) : 0;

                var minute = match.Groups["Minute"].Value.Length > 0 ? int.Parse(match.Groups["Minute"].Value) : 0;
                
                var second = match.Groups["Second"].Value.Length > 0 ? int.Parse(match.Groups["Second"].Value) : 0;

                var millisecond = match.Groups["Millisecond"].Value.Length > 0 ? int.Parse(match.Groups["Millisecond"].Value) : 0;

                var value = new DateTime(year, month, day, hour, minute, second).AddTicks(millisecond);

                return value;
            }

            throw new Exception($"Date/time format is invalid at {s}.");
        }
    }
}
