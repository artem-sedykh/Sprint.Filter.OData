using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Sprint.Filter.OData.Serialize
{   
    internal class PreExpressionTranslator
    {

        internal class PreExpressionTranslatorVisitor : ExpressionVisitor
        {
            internal class ParameterVisitor : ExpressionVisitor
            {
                private readonly IList<ParameterExpression> innerParameters = new List<ParameterExpression>();
                public bool Parameters { get; set; }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (!innerParameters.Contains(node))
                        Parameters = true;

                    return base.VisitParameter(node);
                }

                protected override Expression VisitLambda<T>(Expression<T> node)
                {
                    foreach (var p in node.Parameters)
                        innerParameters.Add(p);

                    return base.VisitLambda(node);
                }                
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                return !ContainsOuterPparameter(node) ? CollapseCapturedOuterVariables(node) : base.VisitMember(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (!ContainsOuterPparameter(node)) //Если нет параметров, то инвокнем метод!            
                {
                    var value = Expression.Lambda(node).Compile().DynamicInvoke();

                    return Expression.Constant(value);
                }                

                return base.VisitMethodCall(node);
            }
            
            private static bool ContainsOuterPparameter(Expression expression)
            {
                var parameterVisitor = new ParameterVisitor();

                parameterVisitor.Visit(expression);

                return parameterVisitor.Parameters;
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

            private static object GetValue(Expression input)
            {
                var objectMember = Expression.Convert(input, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

                return getterLambda();
            }
        }
        
        public Expression Translate(Expression expr)
        {
            var preExpressionTranslatorVisitor = new PreExpressionTranslatorVisitor();

            return preExpressionTranslatorVisitor.Visit(expr);
        }
    }
}
