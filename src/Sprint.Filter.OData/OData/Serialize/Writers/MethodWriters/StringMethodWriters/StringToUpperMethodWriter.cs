using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringToUpperMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string) && (expression.Method.Name == "ToUpper" || expression.Method.Name == "ToUpperInvariant");
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var obj = expression.Object;

            return $"toupper({writer(obj)})";
        }
    }
}
