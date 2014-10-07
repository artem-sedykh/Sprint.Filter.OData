using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sprint.Filter.OData.Deserialize
{
    internal static class MethodProvider
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

        public static MethodInfo ReplaceMethod
        {
            get
            {
                return InnerReplaceMethod;
            }
        }
        public static MethodInfo IndexOfMethod
        {
            get
            {                
                return InnerIndexOfMethod;
            }
        }

        public static MethodInfo ContainsMethod
        {
            get
            {                
                return InnerContainsMethod;
            }
        }

        public static MethodInfo EndsWithMethod
        {
            get
            {                
                return EndsWithMethod1;
            }
        }

        public static MethodInfo StartsWithMethod
        {
            get
            {                
                return InnerStartsWithMethod;
            }
        }

        public static PropertyInfo LengthProperty
        {
            get
            {                
                return InnerLengthProperty;
            }
        }

        public static MethodInfo SubstringMethodWithOneArg
        {
            get
            {                
                return InnerSubstringMethodWithOneArg;
            }
        }
        public static MethodInfo SubstringMethodWithTwoArg
        {
            get
            {                
                return InnerSubstringMethodWithTwoArg;
            }
        }

        public static MethodInfo ConcatMethod
        {
            get
            {                
                return InnerConcatMethod;
            }
        }

        public static MethodInfo ToLowerMethod
        {
            get
            {                
                return InnerToLowerMethod;
            }
        }

        public static MethodInfo ToUpperMethod
        {
            get
            {                
                return InnerToUpperMethod;
            }
        }

        public static MethodInfo TrimMethod
        {
            get
            {                
                return InnerTrimMethod;
            }
        }

        public static PropertyInfo DayProperty
        {
            get
            {                
                return InnerDayProperty;
            }
        }

        public static PropertyInfo HourProperty
        {
            get
            {                
                return InnerHourProperty;
            }
        }

        public static PropertyInfo MinuteProperty
        {
            get
            {                
                return InnerMinuteProperty;
            }
        }

        public static PropertyInfo SecondProperty
        {
            get
            {                
                return InnerSecondProperty;
            }
        }

        public static PropertyInfo MonthProperty
        {
            get
            {                
                return InnerMonthProperty;
            }
        }

        public static PropertyInfo YearProperty
        {
            get
            {                
                return InnerYearProperty;
            }
        }

        public static MethodInfo DoubleRoundMethod
        {
            get
            {                
                return InnerDoubleRoundMethod;
            }
        }

        public static MethodInfo DecimalRoundMethod
        {
            get
            {                
                return InnerDecimalRoundMethod;
            }
        }

        public static MethodInfo DoubleFloorMethod
        {
            get
            {                
                return InnerDoubleFloorMethod;
            }
        }

        public static MethodInfo DecimalFloorMethod
        {
            get
            {                
                return InnerDecimalFloorMethod;
            }
        }

        public static MethodInfo DoubleCeilingMethod
        {
            get
            {                
                return InnerDoubleCeilingMethod;
            }
        }

        public static MethodInfo DecimalCeilingMethod
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
