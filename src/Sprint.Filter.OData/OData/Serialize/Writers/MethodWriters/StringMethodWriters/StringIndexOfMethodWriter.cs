using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringIndexOfMethodWriter : IMethodWriter
    {
        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && expression.Method.Name == "IndexOf";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var argumentExpression = expression.Arguments[0];
            var obj = expression.Object;

            return $"indexof({writer(obj)}, {writer(argumentExpression)})";
        }
    }
}
