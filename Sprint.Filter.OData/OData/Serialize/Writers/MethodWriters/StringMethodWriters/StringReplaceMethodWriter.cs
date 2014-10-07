using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringReplaceMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                   && expression.Method.Name == "Replace";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var firstArgument = expression.Arguments[0];
            var secondArgument = expression.Arguments[1];
            var obj = expression.Object;

            return string.Format("replace({0}, {1}, {2})", writer(obj), writer(firstArgument), writer(secondArgument));
        }
    }
}
