using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class QueryableMethodWriter : IMethodWriter
    {
        public string Write(MethodCallExpression expression, Translator translator)
        {
            var arguments = expression.Arguments.Select(x => translator.Visit(x)).ToList();
            var source = arguments[0];
            arguments.Remove(source);

            return String.Format("{0}/{1}({2})", source, expression.Method.Name, String.Join(", ", arguments));
        }
    }
}
