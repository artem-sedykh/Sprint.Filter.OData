using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sprint.Filter.OData
{
    public static class MethodProvider
    {
        internal static readonly IDictionary<string, MethodInfo[]> UserFunctions = new Dictionary<string, MethodInfo[]>(); 

        static MethodProvider()
        {
            var stringType = typeof(string);
            var datetimeType = typeof(DateTime);
            var mathType = typeof(Math);

            ReplaceMethod = stringType.GetMethod("Replace", new[] { stringType,stringType });
            ContainsMethod = stringType.GetMethod("Contains", new[] { stringType });
            IndexOfMethod = stringType.GetMethod("IndexOf", new[] { stringType });
            EndsWithMethod = stringType.GetMethod("EndsWith", new[] { stringType });
            StartsWithMethod = stringType.GetMethod("StartsWith", new[] { stringType });
            LengthProperty = stringType.GetProperty("Length", Type.EmptyTypes);
            SubstringMethodWithOneArg = stringType.GetMethod("Substring", new[] { typeof(int) });
            SubstringMethodWithTwoArg = stringType.GetMethod("Substring", new[] { typeof(int), typeof(int) });
            ToLowerMethod = stringType.GetMethod("ToLower", Type.EmptyTypes);
            ToUpperMethod = stringType.GetMethod("ToUpper", Type.EmptyTypes);
            TrimMethod = stringType.GetMethod("Trim", Type.EmptyTypes);
            ConcatMethod = stringType.GetMethod("Concat", new[] { stringType, stringType });
            DayProperty = datetimeType.GetProperty("Day", Type.EmptyTypes);
            HourProperty = datetimeType.GetProperty("Hour", Type.EmptyTypes);
            MinuteProperty = datetimeType.GetProperty("Minute", Type.EmptyTypes);
            SecondProperty = datetimeType.GetProperty("Second", Type.EmptyTypes);
            MonthProperty = datetimeType.GetProperty("Month", Type.EmptyTypes);
            YearProperty = datetimeType.GetProperty("Year", Type.EmptyTypes);

            DoubleRoundMethod = mathType.GetMethod("Round", new[] { typeof(double) });
            DecimalRoundMethod = mathType.GetMethod("Round", new[] { typeof(decimal) });
            DoubleFloorMethod = mathType.GetMethod("Floor", new[] { typeof(double) });
            DecimalFloorMethod = mathType.GetMethod("Floor", new[] { typeof(decimal) });
            DoubleCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(double) });
            DecimalCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(decimal) });
        }

        internal static MethodInfo ReplaceMethod { get; }

        internal static MethodInfo IndexOfMethod { get; }

        internal static MethodInfo ContainsMethod { get; }

        internal static MethodInfo EndsWithMethod { get; }

        internal static MethodInfo StartsWithMethod { get; }

        internal static PropertyInfo LengthProperty { get; }

        internal static MethodInfo SubstringMethodWithOneArg { get; }

        internal static MethodInfo SubstringMethodWithTwoArg { get; }

        internal static MethodInfo ConcatMethod { get; }

        internal static MethodInfo ToLowerMethod { get; }

        internal static MethodInfo ToUpperMethod { get; }

        internal static MethodInfo TrimMethod { get; }

        internal static PropertyInfo DayProperty { get; }

        internal static PropertyInfo HourProperty { get; }

        internal static PropertyInfo MinuteProperty { get; }

        internal static PropertyInfo SecondProperty { get; }

        internal static PropertyInfo MonthProperty { get; }

        internal static PropertyInfo YearProperty { get; }

        internal static MethodInfo DoubleRoundMethod { get; }

        internal static MethodInfo DecimalRoundMethod { get; }

        internal static MethodInfo DoubleFloorMethod { get; }

        internal static MethodInfo DecimalFloorMethod { get; }

        internal static MethodInfo DoubleCeilingMethod { get; }

        internal static MethodInfo DecimalCeilingMethod { get; }

        public static void RegisterFunction(string name, MethodInfo[] methodInfos)
        {
            if(UserFunctions.ContainsKey(name))
                throw new Exception($"function '{name}' exists");

            UserFunctions.Add(name, methodInfos);
        }
    }
}
