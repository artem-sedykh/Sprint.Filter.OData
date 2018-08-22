using System;
using System.Linq;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class QueryableMethodWriter : IMethodWriter
    {
        private static readonly Type QueryableType = typeof(Queryable);

        private static readonly Type EnumerableType = typeof(Enumerable);

        public QueryableMethodWriter()
        {
            Priority = 0;
        }

        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.ReflectedType == QueryableType || expression.Method.ReflectedType == EnumerableType;
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var arguments = expression.Arguments.Select(writer).ToList();
            var source = arguments[0];
            arguments.Remove(source);

            return $"{source}/{expression.Method.Name}({string.Join(", ", arguments)})";
        }
    }
}
