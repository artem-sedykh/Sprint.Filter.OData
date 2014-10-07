using System;
using System.Linq;
using System.Linq.Expressions;

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

            return String.Format("{0}/{1}({2})", source, expression.Method.Name, String.Join(", ", arguments));
        }
    }
}
