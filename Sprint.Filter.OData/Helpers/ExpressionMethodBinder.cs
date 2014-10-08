using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.Helpers
{
    public static class ExpressionMethodBinder
    {        
        internal class Method
        {
            public Method(ParameterInfo[] parameters)
            {                
                GenericArguments = new Dictionary<Type, Type>();
            }

            public MethodInfo MethodInfo { get; set; }

            public int Priority { get; set; }

            public IDictionary<Type, Type> GenericArguments { get; set; }

            public ParameterInfo[] MethodParameters { get; set; }
        }

        public static MethodCallExpression Bind(MethodInfo[] methodInfos, string methodName, Expression instance, Expression[] args)
        {
            methodInfos = methodInfos.Where(m => String.Compare(m.Name, methodName, StringComparison.OrdinalIgnoreCase) == 0 && m.GetParameters().Length == args.Length).ToArray();

            var method = FindBestMethod(methodInfos, args);

            if (method == null)
                return null;

            var methodInfo = method.MethodInfo;

            if (method.MethodInfo.IsGenericMethod)
            {
                var genericArgs = methodInfo.GetGenericArguments().Select(x => method.GenericArguments[x]).ToArray();

                methodInfo = methodInfo.MakeGenericMethod(genericArgs);
            }

            var arguments = methodInfo.GetParameters().Select((p, index) => ConverExpression(args[index], p.ParameterType)).ToArray();

            return methodInfo.Attributes.HasFlag(MethodAttributes.Static) ? Expression.Call(methodInfo, arguments) : Expression.Call(instance, methodInfo, arguments);
        }

        public static MethodCallExpression Bind(Type type, string methodName,Expression instance, Expression[] args)
        {
            var methodInfos = type.GetMethods()
                .Where(m => String.Compare(m.Name, methodName, StringComparison.OrdinalIgnoreCase) == 0 && m.GetParameters().Length == args.Length).ToArray();

            return Bind(methodInfos, methodName, instance, args);
        }

        private static Method FindBestMethod(IEnumerable<MethodInfo> methodInfos, Expression[] args)
        {
            if (methodInfos == null)
                throw new ArgumentNullException("methodInfos");

            if (args == null)
                throw new ArgumentNullException("args");


            var methods = new List<Method>();

            foreach(var m in methodInfos)
            {
                var parameters = m.GetParameters();

                var method = new Method(parameters)
                {
                    MethodInfo = m
                };

                methods.Add(method);

                for (var index = 0; index < parameters.Length; index++)
                {
                    var parameterType = parameters[index].ParameterType;

                    if(args[index].IsNullConstant())
                    {
                        if(parameterType.IsNullableType() || !parameterType.IsValueType)
                            continue;
                    }

                    var argumentType = args[index].Type;

                    if(argumentType == parameterType)
                        continue;

                    if(argumentType != parameterType)
                    {
                        if(parameterType.IsGenericType  && !parameterType.IsNullableType())
                        {
                            if(parameterType.BaseType == typeof(LambdaExpression))                            
                                parameterType = parameterType.GetGenericArguments()[0];

                            if (argumentType.BaseType == typeof(LambdaExpression))
                                argumentType = argumentType.GetGenericArguments()[0];

                            var cast = false;

                            if(CanConvertGenericParameterType(parameterType, argumentType, method.GenericArguments, ref cast))
                            {
                                if(cast)
                                    method.Priority--;

                                continue;
                            }
                        }
                        else
                        {
                            if(CanConvert(method, parameterType, argumentType))
                            {
                                method.Priority--;
                                continue;
                            }                                
                        }

                        methods.Remove(method);
                        break;
                    }                    
                }

                if(method.MethodInfo.GetGenericArguments().Count(x => x.IsGenericParameter) != method.GenericArguments.Count)
                    methods.Remove(method);
            }

            if (methods.Count == 0)
                return null;

            var maxPriority = methods.Max(x => x.Priority);

            if(methods.Count(x => x.Priority == maxPriority) > 1)
            {
                var minGenericArgs = methods.Min(x => x.GenericArguments.Count);

                if (methods.Count(x => x.Priority == maxPriority && x.GenericArguments.Count == minGenericArgs) == 1)
                    return methods.OrderByDescending(x => x.Priority == maxPriority && x.GenericArguments.Count == minGenericArgs).FirstOrDefault();

                throw new Exception(String.Format("The call is ambiguous between the following methods: {0}", String.Join(", ", methods.Where(x => x.Priority == maxPriority).Select(x => x.MethodInfo.ToString()))));
            }


            return methods.OrderByDescending(x => x.Priority).FirstOrDefault(); 
        }

        private static bool CanConvert(Method method, Type parameterType, Type argumentType)
        {
            if (parameterType.IsGenericParameter)
            {
                var constraints = parameterType.GetGenericParameterConstraints();

                if(constraints.All(c => c.IsAssignableFrom(argumentType)))
                {                    
                    if(method.GenericArguments.ContainsKey(parameterType) && method.GenericArguments[parameterType] != argumentType)
                        return false;

                    method.GenericArguments[parameterType] = argumentType;

                    return true;
                }
            }  

            if (parameterType.IsClass)
                return parameterType.IsAssignableFrom(argumentType);

            parameterType = Nullable.GetUnderlyingType(parameterType) ?? parameterType;
            argumentType = Nullable.GetUnderlyingType(argumentType) ?? argumentType;

            return ExpressionHelper.ImplicitNumericConversions.ContainsKey(argumentType) && ExpressionHelper.ImplicitNumericConversions[argumentType].Contains(parameterType); 
        }

        internal static bool CanConvertGenericParameterType(Type parameterType, Type argumentType,
            IDictionary<Type, Type> genericParameters, ref bool cast)
        {
            return CanConvertGenericParameterType(parameterType, argumentType, genericParameters, true, ref cast);
        }

        private static bool CanConvertGenericParameterType(Type parameterType, Type argumentType,
            IDictionary<Type, Type> genericParameters, bool covariantOrContravariantParameter, ref bool cast)
        {
            if (!parameterType.IsGenericType || !(argumentType.IsGenericType || argumentType.IsArray))
                return false;

            var genericParameterType = parameterType.GetGenericTypeDefinition();
            var genericArgumentType = argumentType.IsArray ? argumentType : argumentType.GetGenericTypeDefinition();


            if(genericParameterType != genericArgumentType)
            {
                if (covariantOrContravariantParameter && genericArgumentType.IsAssignableToGenericType(genericParameterType))
                    cast = true;
                else
                    return false;
            }

            var parameterTypeGenericArguments = parameterType.GetGenericArguments();
            var argumentTypeGenericArguments = argumentType.IsArray ? new[] { argumentType.GetElementType() } : argumentType.GetGenericArguments();

            if (parameterTypeGenericArguments.Length != argumentTypeGenericArguments.Length)
                return false;

            for (var i = 0; i < parameterTypeGenericArguments.Length; i++)
            {
                var parameterTypeGenericArgument = parameterTypeGenericArguments[i];
                var argumentTypeGenericArgument = argumentTypeGenericArguments[i];

                if(parameterTypeGenericArgument.IsGenericType  && argumentTypeGenericArgument.IsGenericType)
                {
                    var genericAttributes = genericParameterType.GetGenericArguments()[i].GenericParameterAttributes;

                    var canConvert = genericAttributes.HasFlag(GenericParameterAttributes.Covariant) ||
                                     genericAttributes.HasFlag(GenericParameterAttributes.Contravariant) || genericParameterType == typeof(Expression<>);

                    if (!CanConvertGenericParameterType(parameterTypeGenericArgument, argumentTypeGenericArgument, genericParameters, canConvert, ref cast))
                        return false;
                }
                else
                {
                    if(parameterTypeGenericArgument.IsGenericParameter)
                    {
                        var constraints = parameterTypeGenericArgument.GetGenericParameterConstraints();

                        if(constraints.Any(c => !c.IsAssignableFrom(argumentTypeGenericArgument)))
                            return false;

                        if(genericParameters.ContainsKey(parameterTypeGenericArgument) &&
                           genericParameters[parameterTypeGenericArgument] != argumentTypeGenericArgument)
                            return false;

                        genericParameters[parameterTypeGenericArgument] = argumentTypeGenericArgument;
                    }
                    else
                    {
                        if(parameterTypeGenericArgument != argumentTypeGenericArgument)
                        {
                            var genericAttributes = genericParameterType.GetGenericArguments()[i].GenericParameterAttributes;

                            if(parameterTypeGenericArgument.IsAssignableFrom(argumentTypeGenericArgument) &&
                               genericAttributes.HasFlag(GenericParameterAttributes.Covariant) ||
                               genericAttributes.HasFlag(GenericParameterAttributes.Contravariant))
                            {
                                cast = true;

                                return true;
                            }

                            return false;
                        }                        
                    }
                }
            }            

            return true;
        }

        private static Expression ConverExpression(Expression expression, Type type)
        {

            if(type == expression.Type)
                return expression;

            if (type.BaseType == typeof(LambdaExpression) && expression.NodeType==ExpressionType.Lambda)
            {
                var lambda = (LambdaExpression)expression;

                return Expression.Lambda(type.GetGenericArguments()[0], lambda.Body, lambda.Parameters);
            }

            if(expression.Type.BaseType == typeof(LambdaExpression))
            {
                var lambda = (LambdaExpression)expression;
                return lambda;
            }

            if (!expression.Type.IsValueType && !type.IsValueType)
                return expression;

            if (type.IsGenericParameter)
                return expression;

            if (type.IsGenericType && type.GetGenericArguments().Any(x => x.IsGenericParameter))
            {
                type = type.GetGenericTypeDefinition().MakeGenericType(expression.Type.GenericTypeArguments);

                if (type.IsAssignableFrom(expression.Type))
                    return expression;
            }

            var expr = ExpressionHelper.ConvertExpression(expression, type);

            return expr;
        }
        
    }
}
