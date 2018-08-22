using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sprint.Filter.OData
{
    public static class MethodProvider
    {
        private static readonly MethodInfo InnerContainsMethod;
        private static readonly MethodInfo InnerIndexOfMethod;
        private static readonly MethodInfo EndsWithMethod1;
        private static readonly MethodInfo InnerStartsWithMethod;
        private static readonly PropertyInfo InnerLengthProperty;
        private static readonly MethodInfo InnerSubstringMethodWithOneArg;
        private static readonly MethodInfo InnerSubstringMethodWithTwoArg;
        private static readonly MethodInfo InnerToLowerMethod;
        private static readonly MethodInfo InnerToUpperMethod;
        private static readonly MethodInfo InnerTrimMethod;
        private static readonly PropertyInfo InnerDayProperty;
        private static readonly PropertyInfo InnerHourProperty;
        private static readonly PropertyInfo InnerMinuteProperty;
        private static readonly PropertyInfo InnerSecondProperty;
        private static readonly PropertyInfo InnerMonthProperty;
        private static readonly PropertyInfo InnerYearProperty;
        private static readonly MethodInfo InnerDoubleRoundMethod;
        private static readonly MethodInfo InnerDecimalRoundMethod;
        private static readonly MethodInfo InnerDoubleFloorMethod;
        private static readonly MethodInfo InnerDecimalFloorMethod;
        private static readonly MethodInfo InnerDoubleCeilingMethod;
        private static readonly MethodInfo InnerDecimalCeilingMethod;
        private static readonly MethodInfo InnerConcatMethod;
        private static readonly MethodInfo InnerReplaceMethod;
        internal static readonly IDictionary<string, MethodInfo[]> UserFunctions = new Dictionary<string, MethodInfo[]>(); 

        static MethodProvider()
        {
            var stringType = typeof(string);
            var datetimeType = typeof(DateTime);
            var mathType = typeof(Math);

            InnerReplaceMethod = stringType.GetMethod("Replace", new[] { stringType,stringType });
            InnerContainsMethod = stringType.GetMethod("Contains", new[] { stringType });
            InnerIndexOfMethod = stringType.GetMethod("IndexOf", new[] { stringType });
            EndsWithMethod1 = stringType.GetMethod("EndsWith", new[] { stringType });
            InnerStartsWithMethod = stringType.GetMethod("StartsWith", new[] { stringType });
            InnerLengthProperty = stringType.GetProperty("Length", Type.EmptyTypes);
            InnerSubstringMethodWithOneArg = stringType.GetMethod("Substring", new[] { typeof(int) });
            InnerSubstringMethodWithTwoArg = stringType.GetMethod("Substring", new[] { typeof(int), typeof(int) });
            InnerToLowerMethod = stringType.GetMethod("ToLower", Type.EmptyTypes);
            InnerToUpperMethod = stringType.GetMethod("ToUpper", Type.EmptyTypes);
            InnerTrimMethod = stringType.GetMethod("Trim", Type.EmptyTypes);
            InnerConcatMethod = stringType.GetMethod("Concat", new[] { stringType, stringType });
            InnerDayProperty = datetimeType.GetProperty("Day", Type.EmptyTypes);
            InnerHourProperty = datetimeType.GetProperty("Hour", Type.EmptyTypes);
            InnerMinuteProperty = datetimeType.GetProperty("Minute", Type.EmptyTypes);
            InnerSecondProperty = datetimeType.GetProperty("Second", Type.EmptyTypes);
            InnerMonthProperty = datetimeType.GetProperty("Month", Type.EmptyTypes);
            InnerYearProperty = datetimeType.GetProperty("Year", Type.EmptyTypes);

            InnerDoubleRoundMethod = mathType.GetMethod("Round", new[] { typeof(double) });
            InnerDecimalRoundMethod = mathType.GetMethod("Round", new[] { typeof(decimal) });
            InnerDoubleFloorMethod = mathType.GetMethod("Floor", new[] { typeof(double) });
            InnerDecimalFloorMethod = mathType.GetMethod("Floor", new[] { typeof(decimal) });
            InnerDoubleCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(double) });
            InnerDecimalCeilingMethod = mathType.GetMethod("Ceiling", new[] { typeof(decimal) });
        }

        internal static MethodInfo ReplaceMethod
        {
            get
            {
                return InnerReplaceMethod;
            }
        }
        internal static MethodInfo IndexOfMethod
        {
            get
            {                
                return InnerIndexOfMethod;
            }
        }

        internal static MethodInfo ContainsMethod
        {
            get
            {                
                return InnerContainsMethod;
            }
        }

        internal static MethodInfo EndsWithMethod
        {
            get
            {                
                return EndsWithMethod1;
            }
        }

        internal static MethodInfo StartsWithMethod
        {
            get
            {                
                return InnerStartsWithMethod;
            }
        }

        internal static PropertyInfo LengthProperty
        {
            get
            {                
                return InnerLengthProperty;
            }
        }

        internal static MethodInfo SubstringMethodWithOneArg
        {
            get
            {                
                return InnerSubstringMethodWithOneArg;
            }
        }
        internal static MethodInfo SubstringMethodWithTwoArg
        {
            get
            {                
                return InnerSubstringMethodWithTwoArg;
            }
        }

        internal static MethodInfo ConcatMethod
        {
            get
            {                
                return InnerConcatMethod;
            }
        }

        internal static MethodInfo ToLowerMethod
        {
            get
            {                
                return InnerToLowerMethod;
            }
        }

        internal static MethodInfo ToUpperMethod
        {
            get
            {                
                return InnerToUpperMethod;
            }
        }

        internal static MethodInfo TrimMethod
        {
            get
            {                
                return InnerTrimMethod;
            }
        }

        internal static PropertyInfo DayProperty
        {
            get
            {                
                return InnerDayProperty;
            }
        }

        internal static PropertyInfo HourProperty
        {
            get
            {                
                return InnerHourProperty;
            }
        }

        internal static PropertyInfo MinuteProperty
        {
            get
            {                
                return InnerMinuteProperty;
            }
        }

        internal static PropertyInfo SecondProperty
        {
            get
            {                
                return InnerSecondProperty;
            }
        }

        internal static PropertyInfo MonthProperty
        {
            get
            {                
                return InnerMonthProperty;
            }
        }

        internal static PropertyInfo YearProperty
        {
            get
            {                
                return InnerYearProperty;
            }
        }

        internal static MethodInfo DoubleRoundMethod
        {
            get
            {                
                return InnerDoubleRoundMethod;
            }
        }

        internal static MethodInfo DecimalRoundMethod
        {
            get
            {                
                return InnerDecimalRoundMethod;
            }
        }

        internal static MethodInfo DoubleFloorMethod
        {
            get
            {                
                return InnerDoubleFloorMethod;
            }
        }

        internal static MethodInfo DecimalFloorMethod
        {
            get
            {                
                return InnerDecimalFloorMethod;
            }
        }

        internal static MethodInfo DoubleCeilingMethod
        {
            get
            {                
                return InnerDoubleCeilingMethod;
            }
        }

        internal static MethodInfo DecimalCeilingMethod
        {
            get
            {                
                return InnerDecimalCeilingMethod;
            }
        }
        
        public static void RegisterFunction(string name, MethodInfo[] methodInfos)
        {            
            if(UserFunctions.ContainsKey(name))
                throw new Exception(String.Format("function '{0}' exists", name));

            UserFunctions.Add(name, methodInfos);
        }
    }
}
