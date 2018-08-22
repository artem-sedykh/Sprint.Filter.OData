using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
// ReSharper disable once CheckNamespace

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class ContainsMethodWriter:IMethodWriter
    {
        private static readonly Type QueryableType = typeof(Queryable);

        private static readonly Type EnumerableType = typeof(Enumerable);

        public ContainsMethodWriter()
        {
            Priority = 1;
        }

        public string Write(MethodCallExpression expression, QueryTranslator queryTranslator)
        {
            var source = expression.Arguments[0];
            var item = expression.Arguments[1];
            var identificator = queryTranslator.Visit(item);

            if (source.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)source;

                var values = (from object obj in (IEnumerable)constant.Value select $"{identificator} eq {queryTranslator.VisitConstant(Expression.Constant(obj))}").ToArray();

                return "(" + String.Join(" or ", values) + ")";
            }

            return $"{queryTranslator.Visit(source)}/{expression.Method.Name}({identificator})";
        }

        public int Priority { get; set; }

        public bool CanHandle(MethodCallExpression expression)
        {
            return ((expression.Method.ReflectedType == QueryableType || expression.Method.ReflectedType == EnumerableType) && expression.Method.Name == "Contains");
        }

        public string Write(MethodCallExpression expression, Func<Expression, string> writer)
        {
            var source = expression.Arguments[0];
            var item = expression.Arguments[1];
            var identificator = writer(item);

            if(source.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)source;

                var values = (from object obj in (IEnumerable)constant.Value select $"{identificator} eq {writer(Expression.Constant(obj))}").ToArray();

                return "(" + string.Join(" or ", values) + ")";
            }

            return $"{writer(source)}/{expression.Method.Name}({identificator})";
        }
    }
}
