namespace Sprint.Filter.Extensions
{
    internal static class StringExtensions
    {
        public static string Capitalize(this string input)
        {            
            return char.ToUpperInvariant(input[0]) + input.Substring(1);
        }
    }
}
