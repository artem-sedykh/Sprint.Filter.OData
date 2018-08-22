using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class UserFunctionMethodWriter:IMethodWriter
    {
        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            var methods = MethodProvider.UserFunctions.SelectMany(x => x.Value);
            var methodInfos = methods as MethodInfo[] ?? methods.ToArray();

            if(expression.Method.IsGenericMethod)
            {
                var genericMethodDefinition = expression.Method.GetGenericMethodDefinition();

                return methodInfos.Any(m => m == genericMethodDefinition) || methodInfos.Any(m => m == expression.Method);
            }

            return methodInfos.Any(m => m == expression.Method);
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var arguments = expression.Arguments.Select(writer).ToArray();

            if(expression.Method.IsGenericMethod)
            {
                var genericMethodDefinition = expression.Method.GetGenericMethodDefinition();

                var userFunction = MethodProvider.UserFunctions.First(uf =>uf.Value.Any(m => m == genericMethodDefinition) || uf.Value.Any(m => m == expression.Method));                

                return $"{userFunction.Key}({string.Join(", ", arguments)})";
            }

            var uFunction = MethodProvider.UserFunctions.First(uf => uf.Value.Any(m => m == expression.Method));

            return $"{uFunction.Key}({string.Join(", ", arguments)})";
        }
    }
}
