using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class StringTrimMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }
        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(string)
                   && expression.Method.Name == "Trim";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var obj = expression.Object;            

            return string.Format("trim({0})", writer(obj));
        }
    }
}
