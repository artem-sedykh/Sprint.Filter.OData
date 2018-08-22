using System;
using System.Linq;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class DefaultMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return true;
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var method = expression.Method;

            var arguments = expression.Arguments.Select(writer).ToArray();

            if(method.IsStatic)
                throw new NotSupportedException(expression.ToString());

            var obj = writer(expression.Object);

            return $"{obj}/{method.Name}({string.Join(", ", arguments)})";

        }
    }
}
