using System;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class MathFloorMethodWriter : IMethodWriter
    {
        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            return expression.Method.DeclaringType == typeof(Math) && expression.Method.Name == "Floor";
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var mathArgument = expression.Arguments[0];

            return $"floor({writer(mathArgument)})";
        }
    }
}
