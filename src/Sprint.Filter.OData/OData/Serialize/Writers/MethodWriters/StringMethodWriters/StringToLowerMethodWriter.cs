using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringToLowerMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && (expression.Method.Name == "ToLower" || expression.Method.Name == "ToLowerInvariant");
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var obj = expression.Object;

            return $"tolower({writer(obj)})";
        }
    }
}
