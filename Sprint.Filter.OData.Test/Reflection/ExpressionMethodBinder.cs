using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sprint.Filter.Helpers;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Test.Reflection
{
    public class ExpressionMethodBinder
    {
        private class MethodPriority
        {
            public MethodPriority(ParameterInfo[] parameters)
            {
                Parameters = parameters;

                GenericArguments = new Dictionary<Type, Type>();
            }
            public int Priority { get; set; }//Приоритет выполения метода

            public MethodInfo MethodInfo { get; set; }//Метод

            public ParameterInfo[] Parameters { get; set; }

            public IDictionary<Type,Type> GenericArguments { get; set; }
        }

        private static bool CanConvertGenericArgument(Type methodParameterType, Type parameter, IDictionary<Type, Type> genericArguments, ref bool isAssignable)
        {
            var methodParameterTypeGtd= methodParameterType.GetGenericTypeDefinition();
            var parameterGtd = parameter.GetGenericTypeDefinition();

            
            if (methodParameterTypeGtd != parameterGtd && !parameterGtd.IsAssignableToGenericType(methodParameterTypeGtd))
                return false;


            if(methodParameterTypeGtd != parameterGtd)
            {
                if(!parameterGtd.IsAssignableToGenericType(methodParameterTypeGtd))
                    return false;

                isAssignable = true;
            }

            var closeTypeGenericArguments = parameter.GetGenericArguments();
            var openTypeGenericArguments = methodParameterType.GetGenericArguments();

            if (closeTypeGenericArguments.Length != openTypeGenericArguments.Length)
                return false;

            for (var i = 0; i < closeTypeGenericArguments.Length; i++)
            {
                var openGenericArgument = openTypeGenericArguments[i];
                var closeGenerigArgument = closeTypeGenericArguments[i];

                if (closeGenerigArgument.IsGenericType)
                {
                    if (!CanConvertGenericArgument(openGenericArgument, closeGenerigArgument, genericArguments, ref isAssignable))
                        return false;
                }
                else
                {
                    if (openGenericArgument.IsGenericParameter)
                    {
                        var constraints = openGenericArgument.GetGenericParameterConstraints();

                        if(constraints.Any(c => !c.IsAssignableFrom(closeGenerigArgument)))                        
                            return false;

                        if(genericArguments.ContainsKey(openGenericArgument) &&
                           genericArguments[openGenericArgument] != closeGenerigArgument)
                            return false;

                        genericArguments[openGenericArgument] = closeGenerigArgument;
                    }

                }
            }

            return true;
        }

        public static void PrepareGenericArguments(Type genericParameter, Type type2)
        {
            
        }
        private static MethodPriority GetBestMethod(MethodInfo[] methodInfos, Expression[] arguments)
        {
            if (methodInfos == null)
                throw new ArgumentNullException("methodInfos");

            if (arguments == null)
                throw new ArgumentNullException("arguments");

            var methodsInfoByPriority = new List<MethodPriority>();

            foreach (var methodInfo in methodInfos)
            {                
                var methodParameters = methodInfo.GetParameters().Where(p => !p.IsOut).ToArray();

                if (methodParameters.Length >= arguments.Length)
                {
                    var methodInfoByPriority = new MethodPriority(methodParameters)                    
                    {
                        MethodInfo = methodInfo,
                        Priority = 0
                    };

                    methodsInfoByPriority.Add(methodInfoByPriority);

                    for (var index = 0; index < methodParameters.Length; index++)
                    {
                        var methodParameter = methodParameters[index];

                        if (index >= arguments.Length)
                        {
                            if (methodParameter.IsOptional)//Если параметер по умолчанию
                                continue;

                            methodsInfoByPriority.Remove(methodInfoByPriority);
                            break;
                        }

                        var argument = arguments[index];



                        if (argument.Type.IsGenericType && methodParameter.ParameterType.IsGenericType && methodParameter.ParameterType.GetGenericArguments().Any(x=>x.IsGenericParameter))
                        {
                            var type = argument.Type;

                            if(methodParameter.ParameterType.BaseType == typeof(LambdaExpression))                            
                                type = argument.GetType();
                            var isAssignable = false;
                            if (!CanConvertGenericArgument(methodParameter.ParameterType, type,
                                methodInfoByPriority.GenericArguments, ref isAssignable))
                            {
                                methodsInfoByPriority.Remove(methodInfoByPriority);
                                break;
                            }

                            if(isAssignable)
                                methodInfoByPriority.Priority--;

                            continue;
                        }

                        if(methodParameter.ParameterType.IsGenericParameter)
                        {
                            var constraints = methodParameter.ParameterType.GetGenericParameterConstraints();
                            if(constraints.Any(c => !c.IsAssignableFrom(argument.Type)))
                            {
                                methodsInfoByPriority.Remove(methodInfoByPriority);
                                break;
                            }

                            if(methodInfoByPriority.GenericArguments.ContainsKey(methodParameter.ParameterType) &&
                               methodInfoByPriority.GenericArguments[methodParameter.ParameterType] != argument.Type)
                            {
                                methodsInfoByPriority.Remove(methodInfoByPriority);
                                break;
                            }

                            methodInfoByPriority.GenericArguments[methodParameter.ParameterType] = argument.Type;
                            continue;
                            
                        }


                        if (argument.Type == methodParameter.ParameterType)
                            continue;

                        if (argument.Type != methodParameter.ParameterType && CanConvert(methodParameter.ParameterType, argument.Type))
                        {
                            methodInfoByPriority.Priority--;//Уменьшаем значение приоритета
                            continue;
                        }

                        methodsInfoByPriority.Remove(methodInfoByPriority);//Праметер не подошел, выходим
                        break;
                    }
                }
            }

            if(methodsInfoByPriority.Count == 0)
                return null;

            var maxPriority = methodsInfoByPriority.Max(x => x.Priority);

            if(methodsInfoByPriority.Count(x=>x.Priority==maxPriority)>1)
                throw new Exception(String.Format("The call is ambiguous between the following methods: {0}", String.Join(", ", methodsInfoByPriority.Where(x => x.Priority == maxPriority).Select(x => x.MethodInfo.ToString()))));


            return methodsInfoByPriority.OrderByDescending(x => x.Priority).FirstOrDefault(); 
        }

        public static MethodCallExpression BindMethod(MethodInfo[] methodInfos, Expression[] arguments)
        {            
            if(methodInfos==null)
                throw new ArgumentNullException("methodInfos");

            if (arguments == null)
                throw new ArgumentNullException("arguments");

            var method = GetBestMethod(methodInfos, arguments);

            if(method==null)
                throw new Exception();

            var methodInfo = method.MethodInfo;

            if(methodInfo.IsGenericMethod)
            {
                var genericArgs = methodInfo.GetGenericArguments().Select(x => method.GenericArguments[x]).ToArray();

                methodInfo = methodInfo.MakeGenericMethod(genericArgs);
            }

           arguments= methodInfo.GetParameters().Select((p, index) =>
                    index >= arguments.Length
                        ? Expression.Constant(p.DefaultValue, p.ParameterType)
                        : ConverExpression(arguments[index], p.ParameterType)).ToArray();          
            
            var expression = Expression.Call(methodInfo, arguments);

            return expression;
        }


        public static Expression ConverExpression(Expression expression, Type type )
        {
            if (type.BaseType == typeof(LambdaExpression) && expression is LambdaExpression)
            {
                var lambda = (LambdaExpression)expression;

                return Expression.Lambda(type.GetGenericArguments()[0], lambda.Body, lambda.Parameters);
            }


            if (!expression.Type.IsValueType && !type.IsValueType)//Ссылочные типы не приводим
                return expression;

            if(type.IsGenericParameter)
                return expression;

            if (type.IsGenericType && type.GetGenericArguments().Any(x => x.IsGenericParameter))//IEnumerable<TModel>
            {
                type = type.GetGenericTypeDefinition().MakeGenericType(expression.Type.GenericTypeArguments);

                if(type.IsAssignableFrom(expression.Type))
                    return expression;
            }

            var expr = ExpressionHelper.ConvertExpression(expression, type);

            return expr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodParameterType">Тип параметра метода</param>
        /// <param name="parameterType">Тип аргумента передоваемого в метод</param>
        /// <returns></returns>
        private static bool  CanConvert(Type methodParameterType, Type parameterType)
        {
            if(methodParameterType.IsGenericType && methodParameterType.IsAssignableToGenericType(parameterType))
                return true;

            if (methodParameterType.IsAssignableFrom(parameterType))            
                return true;

            methodParameterType = Nullable.GetUnderlyingType(methodParameterType) ?? methodParameterType;
            parameterType = Nullable.GetUnderlyingType(parameterType) ?? parameterType;

            return ExpressionHelper.ImplicitNumericConversions.ContainsKey(parameterType) &&
                   ExpressionHelper.ImplicitNumericConversions[parameterType].Contains(methodParameterType);            
        }
    }
}
