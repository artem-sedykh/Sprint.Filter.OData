using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Sprint.Filter.OData.Serialize
{
    internal class ExpressionPreparator: ExpressionVisitor
    {
        private static readonly ParameterVisitor ParameterVisitor = new ParameterVisitor();

        internal static bool HasOuterParameters(Expression expression)
        {
            ParameterVisitor.Reset();

            ParameterVisitor.Visit(expression);

            return ParameterVisitor.ContainsOuterParameters;
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            return !HasOuterParameters(node) ? CollapseCapturedOuterVariables(node) : base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!HasOuterParameters(node))
            {
                var value = Expression.Lambda(node).Compile().DynamicInvoke();

                return Expression.Constant(value);
            }

            return base.VisitMethodCall(node);
        }

        private static object GetValue(Expression input)
        {
            var objectMember = Expression.Convert(input, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

            return getterLambda();
        }

        private static Expression CollapseCapturedOuterVariables(MemberExpression input)
        {
            if (input == null || input.NodeType != ExpressionType.MemberAccess)
                return input;

            if (input.Expression == null)
            {
                var val = GetValue(input);

                return Expression.Constant(val);
            }

            switch (input.Expression.NodeType)
            {
                case ExpressionType.New:
                case ExpressionType.MemberAccess:
                    var value = GetValue(input);
                    return Expression.Constant(value);
                case ExpressionType.Constant:
                    var obj = ((ConstantExpression)input.Expression).Value;
                    if (obj == null)
                    {
                        return input;
                    }

                    var fieldInfo = input.Member as FieldInfo;
                    if (fieldInfo != null)
                    {
                        var result = fieldInfo.GetValue(obj);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    var propertyInfo = input.Member as PropertyInfo;
                    if (propertyInfo != null)
                    {
                        var result = propertyInfo.GetValue(obj, null);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    break;
                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return Expression.Constant(GetValue(input));
            }

            return input;
        }
    }

    internal class ParameterVisitor : ExpressionVisitor
    {
        private readonly List<ParameterExpression> innerParameters = new List<ParameterExpression>();
        public bool ContainsOuterParameters { get; private set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (!innerParameters.Contains(node))
                ContainsOuterParameters = true;

            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            foreach (var p in node.Parameters)
                innerParameters.Add(p);

            return base.VisitLambda(node);
        }

        public void Reset()
        {
            ContainsOuterParameters = false;

            innerParameters.Clear();
        }
    }
}
