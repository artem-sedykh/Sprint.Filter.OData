using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.Extensions
{
    internal static class ExpressionTypeExtensions
    {
        private static readonly ExpressionType[] BinaryOperators = {
            ExpressionType.Add,
            ExpressionType.Subtract,
            ExpressionType.Multiply,
            ExpressionType.Divide,
            ExpressionType.Modulo,
            ExpressionType.AndAlso,
            ExpressionType.OrElse,
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.Equal,
            ExpressionType.NotEqual
        };

        private static readonly ExpressionType[] Operators =
        {
            ExpressionType.Add,
            ExpressionType.Subtract,
            ExpressionType.Multiply,
            ExpressionType.Divide,
            ExpressionType.Modulo,
            ExpressionType.Not,
            ExpressionType.Negate,
            ExpressionType.AndAlso,
            ExpressionType.OrElse,
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.Equal,
            ExpressionType.NotEqual
        };

        public static bool IsBinaryLogical(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsCompare(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.NotEqual:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsArithmetic(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.Subtract:
                case ExpressionType.Negate:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsBinary(this ExpressionType expressionType)
        {
            return BinaryOperators.Contains(expressionType);
        }

        public static bool IsOperator(this ExpressionType type)
        {
            return Operators.Contains(type);
        }

        public static int Priority(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.OrElse:
                    return 0;
                case ExpressionType.AndAlso:
                    return 1;
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return 2;
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    return 3;
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                    return 4;
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                    return 5;

                case ExpressionType.Not:
                case ExpressionType.Negate:
                    return 6;
                default:
                    return -1;
            }
        }

        public static bool IsLeftAssociative(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Not:
                case ExpressionType.Negate:
                    return false;
                default:
                    return true;
            }
        }

        public static string DebugFormat(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Add:
                    return "({0} + {1})";
                case ExpressionType.Subtract:
                    return "({0} - {1})";

                case ExpressionType.Multiply:
                    return "({0} * {1})";
                case ExpressionType.Divide:
                    return "({0} / {1})";
                case ExpressionType.Modulo:
                    return "({0} % {1})";

                case ExpressionType.Not:
                    return "Not({0})";
                case ExpressionType.Negate:
                    return "-{0}";

                case ExpressionType.AndAlso:
                    return "({0} AndAlso {1})";
                case ExpressionType.OrElse:
                    return "({0} OrElse {1})";

                case ExpressionType.GreaterThan:
                    return "({0} > {1})";
                case ExpressionType.GreaterThanOrEqual:
                    return "({0} >= {1})";
                case ExpressionType.LessThan:
                    return "({0} < {1})";
                case ExpressionType.LessThanOrEqual:
                    return "({0} <= {1})";
                case ExpressionType.Equal:
                    return "({0} == {1})";
                case ExpressionType.NotEqual:
                    return "({0} != {1})";

                default:
                    throw new NotSupportedException();
            }
        }

        public static string ODataFormat(this ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Add:
                    return "{0} add {1}";
                case ExpressionType.Subtract:
                    return "{0} sub {1}";

                case ExpressionType.Multiply:
                    return "{0} mul {1}";
                case ExpressionType.Divide:
                    return "{0} div {1}";
                case ExpressionType.Modulo:
                    return "{0} mod {1}";

                case ExpressionType.Not:
                    return "not {0}";
                case ExpressionType.Negate:
                    return "-{0}";

                case ExpressionType.AndAlso:
                    return "{0} and {1}";
                case ExpressionType.OrElse:
                    return "{0} or {1}";

                case ExpressionType.GreaterThan:
                    return "{0} gt {1}";
                case ExpressionType.GreaterThanOrEqual:
                    return "{0} ge {1}";
                case ExpressionType.LessThan:
                    return "{0} lt {1}";
                case ExpressionType.LessThanOrEqual:
                    return "{0} le {1}";
                case ExpressionType.Equal:
                    return "{0} eq {1}";
                case ExpressionType.NotEqual:
                    return "{0} ne {1}";

                default:
                    throw new NotSupportedException(type.ToString());
            }
        }
    }
}
