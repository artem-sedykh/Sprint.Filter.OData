using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class MathRoundMethodWriter : IMethodWriter
    {
        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {            
            return expression.Method.DeclaringType == typeof(Math) && expression.Method.Name == "Round";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var mathArgument = expression.Arguments[0];

            return string.Format("{0}({1})", "round", writer(mathArgument));
        }   
    }
}
