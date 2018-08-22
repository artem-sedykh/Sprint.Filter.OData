using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    public class StringEndsWithMethodWriter : IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && expression.Method.Name == "EndsWith";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var argumentExpression = expression.Arguments[0];
            var obj = expression.Object;

            return $"endswith({writer(obj)}, {writer(argumentExpression)})";
        }
    }
}
