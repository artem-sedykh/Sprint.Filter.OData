using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringSubstringMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && expression.Method.Name == "Substring";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var obj = expression.Object;

            if (expression.Arguments.Count == 1)
            {
                var argumentExpression = expression.Arguments[0];

                return $"substring({writer(obj)}, {writer(argumentExpression)})";
            }

            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];

            return $"substring({writer(obj)}, {writer(firstArgument)}, {writer(secondArgument)})";
        }
    }
}
