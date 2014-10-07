using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sprint.Filter.Extensions;
using Sprint.Filter.Helpers;
using Sprint.Filter.OData.Common;

namespace Sprint.Filter.OData.Deserialize
{
    internal class Translator
    {
        private static readonly Expression OrdinalStringComparisonConstant = Expression.Constant(StringComparison.Ordinal);

        private static readonly MethodInfo StringCompareMethodInfo = typeof(string).GetMethod("Compare", new[] { typeof(string), typeof(string), typeof(StringComparison) });
        private static readonly Expression ZeroConstant = Expression.Constant(0);
        private readonly IDictionary<ODataParameterExpression, ParameterExpression> parameters = new Dictionary<ODataParameterExpression, ParameterExpression>();
        private readonly IMemberNameProvider memberNameProvider = new MemberNameProvider();
        private Expression VisitMember(ODataPropertyExpression node)
        {
            if(node.Expression != null)
            {
                var expression = Visit(node.Expression);

                var info = memberNameProvider.ResolveAlias(expression.Type, node.Name);

                if(info.MemberType == MemberTypes.Field)
                    return Expression.Field(expression, (FieldInfo)info);

                if(info.MemberType == MemberTypes.Property)
                    return  Expression.Property(expression, (PropertyInfo)info);                               
            }

            throw new NotImplementedException();         
        }

        private Expression VisitUnary(ODataUnaryExpression node)
        {
            var operand = Visit(node.Operand);

            return Expression.MakeUnary(node.NodeType, operand, node.Type);
        }

        private Expression VisitMethodCall(ODataMethodCallExpression node)
        {
            var functionName = node.MethodName.ToLowerInvariant();

            switch (functionName)
            {
                case "substringof":
                    {
                        var right = Visit(node.Arguments[1]);
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(right, MethodProvider.ContainsMethod, new[] { left });
                    }
                case "endswith":
                    {
                        var right = Visit(node.Arguments[1]);
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.EndsWithMethod, new[] { right });
                    }

                case "startswith":
                    {
                        var right = Visit(node.Arguments[1]);
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.StartsWithMethod, new[] { right });
                    }

                case "length":
                    {
                        var left = Visit(node.Arguments[0]);

                        return Expression.Property(left, MethodProvider.LengthProperty);
                    }

                case "indexof":
                    {
                        var right = Visit(node.Arguments[1]);
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.IndexOfMethod, new[] { right });
                    }

                case "substring":
                    {
                        if (node.Arguments.Length == 3)
                            return Expression.Call(Visit(node.Arguments[0]), MethodProvider.SubstringMethodWithTwoArg, new[] { Visit(node.Arguments[1]), Visit(node.Arguments[2]) });

                        if (node.Arguments.Length == 2)
                            return Expression.Call(Visit(node.Arguments[0]), MethodProvider.SubstringMethodWithOneArg, new[] { Visit(node.Arguments[1]) });

                        throw new NotSupportedException();//Нет перегрузки для данного метода
                    }

                case "tolower":
                    {
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.ToLowerMethod);
                    }

                case "toupper":
                    {
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.ToUpperMethod);
                    }

                case "trim":
                    {
                        var left = Visit(node.Arguments[0]);

                        return Expression.Call(left, MethodProvider.TrimMethod);
                    }

                case "hour":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.HourProperty);
                    }

                case "minute":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.MinuteProperty);
                    }

                case "second":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.SecondProperty);
                    }

                case "day":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.DayProperty);
                    }
                case "month":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.MonthProperty);
                    }
                case "year":
                    {

                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Property(left, MethodProvider.YearProperty);
                    }
                case "round":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleRoundMethod : MethodProvider.DecimalRoundMethod, new[] { left });
                    }
                case "floor":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleFloorMethod : MethodProvider.DecimalFloorMethod, new[] { left });
                    }
                case "ceiling":
                    {
                        var left = Visit(node.Arguments[0]);

                        if (left.Type.IsNullableType())
                            left = Expression.Property(left, "Value");

                        return Expression.Call(left.Type == typeof(double) ? MethodProvider.DoubleCeilingMethod : MethodProvider.DecimalCeilingMethod, new[] { left });
                    }
                case "concat":
                    {
                        var right = Visit(node.Arguments[1]);
                        var left = Visit(node.Arguments[0]);

                        return Expression.Add(left, right, MethodProvider.ConcatMethod);
                    }
                case "replace":
                    {
                        return Expression.Call(Visit(node.Arguments[0]), MethodProvider.ReplaceMethod, new[] { Visit(node.Arguments[1]), Visit(node.Arguments[2]) });
                    }
                case "sum":
                case "average":
                    {
                        var context = Visit(node.Context);

                        var genericArguments = context.Type.GenericTypeArguments.ToArray();

                        var arguments = node.Arguments.Select(argument => argument.NodeType == ExpressionType.Lambda ? VisitLambda((ODataLambdaExpression)argument, genericArguments[0]) : Visit(argument)).ToList();

                        arguments.Insert(0, context);

                        var type = context.Type.IsIQueryable() ? typeof(Queryable) : typeof(Enumerable);

                        return Expression.Call(type, functionName, arguments.Count == 1 ? Type.EmptyTypes : genericArguments, arguments.ToArray());
                    }
                case "min":
                case "max":
                case "selectmany":
                    {
                        var context = Visit(node.Context);

                        var genericArguments = context.Type.GenericTypeArguments.ToArray();

                        var arguments = node.Arguments.Select(argument => argument.NodeType == ExpressionType.Lambda ? VisitLambda((ODataLambdaExpression)argument, genericArguments[0]) : Visit(argument)).ToList();

                        arguments.Insert(0, context);

                        var type = context.Type.IsIQueryable() ? typeof(Queryable) : typeof(Enumerable);

                        var expr = ExpressionMethodBinder.Bind(type, functionName, null, arguments.ToArray());

                        return expr;
                    }
                case "groupby":
                case "orderby":
                case "orderbydescending":
                case "thenby":
                case "thenbydescending":
                case "select":
                    {
                        var context = Visit(node.Context);

                        var genericArguments = context.Type.GenericTypeArguments.ToList();

                        var arguments = node.Arguments.Select(argument => argument.NodeType == ExpressionType.Lambda ? VisitLambda((ODataLambdaExpression)argument, genericArguments[0]) : Visit(argument)).ToList();

                        genericArguments.AddRange(arguments.SelectMany(ExtractTypeFormExpression));

                        arguments.Insert(0, context);

                        var type = context.Type.IsIQueryable() ? typeof(Queryable) : typeof(Enumerable);

                        return Expression.Call(type, functionName, genericArguments.ToArray(), arguments.ToArray());
                    }
                case "any":
                case "all":
                case "first":
                case "where":
                case "take":
                case "skip":
                case "distinct":
                case "last":
                case "lastordefault":
                case "single":
                case "singleordefault":
                case "defaultifempty":
                case "count":
                case "longcount":
                case "firstordefault":
                    {
                        var context = Visit(node.Context);

                        var genericArguments = context.Type.GenericTypeArguments.ToArray();

                        var arguments = node.Arguments.Select(argument => argument.NodeType == ExpressionType.Lambda ? VisitLambda((ODataLambdaExpression)argument, genericArguments[0]) : Visit(argument)).ToList();

                        arguments.Insert(0, context);

                        var type = context.Type.IsIQueryable() ? typeof(Queryable) : typeof(Enumerable);

                        return Expression.Call(type, functionName, genericArguments, arguments.ToArray());
                    }
                default:
                {
                    if(node.Context != null)
                    {
                        var context = Visit(node.Context);

                        var arguments = node.Arguments.Select(Visit).ToList();

                        var expr = ExpressionMethodBinder.Bind(context.Type, functionName, context, arguments.ToArray());

                        if (expr == null)
                            throw new Exception(String.Format("Can't find a method: {0}", node.DebugView()));

                        return expr;
                    }

                    if (MethodProvider.UserFunctions.ContainsKey(node.MethodName))
                    {
                        var arguments = node.Arguments.Select(Visit).ToArray();

                        var methodExpression = ExpressionMethodBinder.Bind(
                            MethodProvider.UserFunctions[node.MethodName], node.MethodName, null, arguments);

                        if (methodExpression == null)
                            throw new Exception(String.Format("Can't find a method: {0}", node.DebugView()));

                        return methodExpression;
                    }

                    throw new NotSupportedException(String.Format("Can't find a method: {0}", node.DebugView()));   
                }                    
            }
        }

        private static Type[] ExtractTypeFormExpression(Expression expression)
        {

            var type = (expression.NodeType == ExpressionType.Lambda)
                ? ((LambdaExpression)expression).Body.Type
                : expression.Type;

            return type.IsGenericType ? type.GetGenericArguments() : new[] { type };
        }

        private Expression VisitConstant(ODataConstantExpression node)
        {
            return Expression.Constant(node.Value);                        
        }

        private Expression VisitBinary(ODataBinaryExpression node)
        {
            var left = Visit(node.Left);
            var right = Visit(node.Right);            

            ExpressionHelper.BinaryExpressionArgumentConverter(ref left, ref right, node.NodeType);

            if(left.Type == typeof(string) || right.Type == typeof(string))
            {
                switch(node.NodeType)
                {
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                        left = Expression.Call(StringCompareMethodInfo, new[] { left, right, OrdinalStringComparisonConstant });
                        right = ZeroConstant;
                        break;
                }
            }

            return Expression.MakeBinary(node.NodeType, left, right);            
        }

        private Expression VisitParameter(ODataParameterExpression node, Type parameterType)
        {
            if(parameters.ContainsKey(node))
                return parameters[node];

            return parameters[node] = Expression.Parameter(parameterType, node.Name);
        }

        private Expression VisitLambda(ODataLambdaExpression node, Type parameterType = null)
        {
            var param = node.Parameters.Select(p=>VisitParameter(p, parameterType)).Cast<ParameterExpression>().ToArray();

            var body = Visit(node.Body);

            return Expression.Lambda(body, param);
        }

        private Expression Visit(ODataExpression expression)
        {
            if(expression == null)
                return null;

            switch(expression.NodeType)
            {
                case ExpressionType.Call:
                    return VisitMethodCall((ODataMethodCallExpression)expression);
                case ExpressionType.Constant:                
                    return VisitConstant((ODataConstantExpression)expression);                                    
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.NotEqual:
                case ExpressionType.Equal:                
                    return VisitBinary((ODataBinaryExpression)expression);                                    
                case ExpressionType.Lambda:                
                    return  VisitLambda((ODataLambdaExpression)expression);                                    
                case ExpressionType.MemberAccess:                
                    return VisitMember((ODataPropertyExpression)expression);                                    
                case ExpressionType.Parameter:
                    return VisitParameter((ODataParameterExpression)expression, null);
                case ExpressionType.Not:
                case ExpressionType.Negate:
                    return VisitUnary((ODataUnaryExpression)expression);
            }

            return null;
        }

        public Expression<Func<TModel, TResult>> Translate<TModel, TResult>(ODataLambdaExpression expression)
        {
            var lambda = (LambdaExpression)VisitLambda(expression, typeof(TModel));

            return Expression.Lambda<Func<TModel, TResult>>(lambda.Body, lambda.Parameters);            
        }

        public Expression<Func<TResult>> Translate<TResult>(ODataLambdaExpression expression)
        {
            expression.Parameters = new ODataParameterExpression[0];

            return (Expression<Func<TResult>>)VisitLambda(expression);
        }
    }
}
