using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.Helpers
{
    internal static class ExpressionHelper
    {
        private static readonly Type NullableType = typeof(Nullable<>);        

        /// <summary>
        /// http://msdn.microsoft.com/en-US/en-en/library/exx3b86w.aspx
        /// </summary>
        internal static readonly Type[] IntegralTypes =
        {
            typeof(sbyte), typeof(byte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong)
        };

        /// <summary>
        /// Implicit Numeric Conversions Table (C# Reference)
        /// http://msdn.microsoft.com/en-US/en-en/library/y5b434w4.aspx
        /// </summary>
        internal static readonly IDictionary<Type, Type[]> ImplicitNumericConversions = new Dictionary<Type, Type[]>
        {
            { typeof(sbyte), new[] { typeof(int), typeof(short), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(byte), new[] { typeof(ushort), typeof(int), typeof(short), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(short), new[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(ushort), new[] { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(int), new[] { typeof(int), typeof(long),typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(uint), new[] { typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(long), new[] { typeof(long),typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(char), new[] { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },            
            { typeof(ulong), new[] { typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(float), new[] { typeof(float), typeof(double),typeof(decimal) } },
            { typeof(double), new[] { typeof(double), typeof(decimal) } },
            { typeof(decimal), new[] { typeof(decimal) } }
        };

        internal static readonly IDictionary<Type, Type> ArifmeticResultTypes = new Dictionary<Type, Type>
        {
            { typeof(sbyte), typeof(int) },
            { typeof(byte), typeof(int) },
            { typeof(short), typeof(int) },
            { typeof(ushort), typeof(int) },
            { typeof(char), typeof(int) },

            { typeof(sbyte?), typeof(int?) },
            { typeof(byte?), typeof(int?) },
            { typeof(short?), typeof(int?) },
            { typeof(ushort?), typeof(int?) },
            { typeof(char?), typeof(int?) }
        };

        internal static Expression ConvertExpression([NotNull]Expression expression, Type type)
        {
            if(expression.Type == type)
                return expression;

            if(expression.NodeType != ExpressionType.Constant || expression.Type == typeof(char))
                return Expression.Convert(expression, type);

            if(type.IsNullableType())
            {
                var value = ((ConstantExpression)expression).Value;

                if(value != null)
                {
                    var genericType = type.GetGenericArguments().First();

                    value = Convert.ChangeType(value, genericType);

                    return Expression.Convert(Expression.Constant(value), type);
                }

                return Expression.Constant(null, type);
            }

            if(expression.Type.IsPrimitive)
            {
                var constant = Convert.ChangeType(((ConstantExpression)expression).Value, type);

                return Expression.Constant(constant, type);
            }

            return Expression.Convert(expression, type);

        }

        internal static Expression ConvertToEnumUnderlyingType(Expression expression, Type enumType, Type enumUnderlyingType, bool liftToNull)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {                
                var constantExpression = (ConstantExpression)expression;

                if(constantExpression.Value == null)
                    return liftToNull ? Expression.Convert(Expression.Constant(null, enumType.ToNullable()), enumUnderlyingType.ToNullable()) : expression;

                if(liftToNull)
                    return Expression.Convert(expression, enumUnderlyingType.ToNullable());
                
                var value = Convert.ChangeType(constantExpression.Value, enumUnderlyingType);
                
                return Expression.Constant(value, enumUnderlyingType);
            }

            if (expression.Type == enumType)
                return Expression.Convert(expression, enumUnderlyingType);

            if (Nullable.GetUnderlyingType(expression.Type) == enumType)
                return Expression.Convert(expression, enumUnderlyingType.ToNullable());

            throw new NotSupportedException();
        }

        private static void BinaryExpressionEnumArgumentConverter(ref Expression left, ref Expression right)
        {
            var leftUnderlyingType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
            var rightUnderlyingType = Nullable.GetUnderlyingType(right.Type) ?? right.Type;

            if (leftUnderlyingType.IsEnum || rightUnderlyingType.IsEnum)
            {
                var liftToNull = left.Type.IsNullableType() || right.Type.IsNullableType();

                var enumType = leftUnderlyingType.IsEnum ? leftUnderlyingType : rightUnderlyingType;

                var enumUnderlyingType = Enum.GetUnderlyingType(enumType);

                if (ArifmeticResultTypes.ContainsKey(enumUnderlyingType))
                    enumUnderlyingType = ArifmeticResultTypes[enumUnderlyingType];

                left = ConvertToEnumUnderlyingType(left, enumType, enumUnderlyingType, liftToNull);
                right = ConvertToEnumUnderlyingType(right, enumType, enumUnderlyingType, liftToNull);
            }
        }

        public static void BinaryExpressionArgumentConverter(ref Expression left, ref Expression right,
           ExpressionType type)
        {
            BinaryExpressionEnumArgumentConverter(ref left, ref right);

            PreparingNullableArguments(ref left, ref right);

            if (type.IsBinary())
            {
                var leftTypeIsNullable = left.Type.IsNullableType();
                var rightTypeIsNullable = right.Type.IsNullableType();

                var logicalOperation = type.IsBinaryLogical() || type.IsCompare();

                var arifmeticOperation = type.IsArithmetic();

                #region Arifmetic operation

                if (arifmeticOperation)
                {
                    if (left.Type == right.Type && ArifmeticResultTypes.ContainsKey(left.Type))
                    {
                        var commonType = ArifmeticResultTypes[left.Type];

                        left = ConvertExpression(left, commonType);
                        right = ConvertExpression(right, commonType);
                    }
                    else
                    {
                        var leftType = Nullable.GetUnderlyingType(left.Type);
                        var rightType = Nullable.GetUnderlyingType(right.Type);

                        if ((leftTypeIsNullable && leftType == right.Type && ArifmeticResultTypes.ContainsKey(right.Type))
                           ||
                           (rightTypeIsNullable && rightType == left.Type && ArifmeticResultTypes.ContainsKey(left.Type)))
                        {
                            if (leftTypeIsNullable)
                            {
                                var commonType = ArifmeticResultTypes[left.Type];

                                if (!rightTypeIsNullable)
                                {
                                    var commonUnderlyingType = commonType.GetGenericArguments().First();
                                    right = ConvertExpression(right, commonUnderlyingType);
                                }

                                right = ConvertExpression(right, commonType);
                                left = ConvertExpression(left, commonType);
                            }
                            else
                            {
                                var commonType = ArifmeticResultTypes[right.Type];
                                var commonUnderlyingType = commonType.GetGenericArguments().First();

                                left = ConvertExpression(left, commonUnderlyingType);
                                left = ConvertExpression(left, commonType);
                                right = ConvertExpression(right, commonType);
                            }
                        }
                    }
                }

                #endregion

                if (logicalOperation && left.Type == right.Type)
                    return;

                if (logicalOperation && left.Type == right.Type)
                    return;

                if (leftTypeIsNullable || rightTypeIsNullable)
                {
                    var leftType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    var rightType = Nullable.GetUnderlyingType(right.Type) ?? right.Type;

                    if (leftType == rightType)
                    {
                        var commonType = NullableType.MakeGenericType(leftType);
                        left = ConvertExpression(left, commonType);
                        right = ConvertExpression(right, commonType);
                    }
                    else
                    {
                        if (ImplicitNumericConversions.ContainsKey(leftType) &&
                           ImplicitNumericConversions.ContainsKey(rightType))
                        {
                            var leftTypes = ImplicitNumericConversions[leftType];
                            var rightTypes = ImplicitNumericConversions[rightType];

                            var isIntegralTypes = IntegralTypes.Contains(left.Type) &&
                                                  IntegralTypes.Contains(right.Type);

                            var commonType = isIntegralTypes
                                ? leftTypes.Where(t => IntegralTypes.Contains(t))
                                    .Intersect(rightTypes.Where(t => IntegralTypes.Contains(t)))
                                    .FirstOrDefault()
                                : leftTypes.Intersect(rightTypes).FirstOrDefault();

                            if (commonType != null)
                            {
                                commonType = NullableType.MakeGenericType(commonType);

                                left = ConvertExpression(left, commonType);
                                right = ConvertExpression(right, commonType);
                            }
                        }
                    }
                }
                else
                {
                    if (ImplicitNumericConversions.ContainsKey(left.Type) &&
                       ImplicitNumericConversions.ContainsKey(right.Type))
                    {
                        var leftTypes = ImplicitNumericConversions[left.Type];
                        var rightTypes = ImplicitNumericConversions[right.Type];

                        var isIntegralTypes = IntegralTypes.Contains(left.Type) && IntegralTypes.Contains(right.Type);

                        var commonType = isIntegralTypes
                            ? leftTypes.Where(t => IntegralTypes.Contains(t))
                                .Intersect(rightTypes.Where(t => IntegralTypes.Contains(t)))
                                .FirstOrDefault()
                            : leftTypes.Intersect(rightTypes).FirstOrDefault();

                        if (commonType != null)
                        {
                            left = ConvertExpression(left, commonType);
                            right = ConvertExpression(right, commonType);
                        }
                    }
                }
            }
        }        

        private static void PreparingNullableArguments(ref Expression left, ref Expression right)
        {
            if (left.IsNullConstant() && right.Type.IsValueType)
            {
                if (right.Type.IsNullableType())
                    left = Expression.Constant(null, right.Type);
                else
                {
                    var type = NullableType.MakeGenericType(right.Type);
                    right = Expression.Convert(right, type);
                    left = Expression.Constant(null, right.Type);
                }
            }

            if (right.IsNullConstant() && left.Type.IsValueType)
            {
                if (left.Type.IsNullableType())
                    right = Expression.Constant(null, left.Type);
                else
                {
                    var type = NullableType.MakeGenericType(left.Type);
                    left = Expression.Convert(left, type);
                    right = Expression.Constant(null, left.Type);
                }
            }
        }
    }
}
