using System;
using System.Linq;
using System.Linq.Expressions;

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
                return String.Format("{0}({1})", method.Name, String.Join(", ", arguments));


            var obj = writer(expression.Object);

            return String.Format("{0}/{1}({2})", obj, method.Name, String.Join(", ", arguments));

        }
    }
}
