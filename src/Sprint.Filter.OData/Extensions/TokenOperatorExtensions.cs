using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.Extensions
{
    internal static class TokenOperatorExtensions
    {
        private static readonly string[] Operations = { "eq", "ne", "gt", "ge", "lt", "le", "and", "or", "not" };
        private static readonly string[] Combiners = { "and", "or" };
        private static readonly string[] Arithmetic = { "add", "sub", "mul", "div", "mod" };
        private static readonly string[] UnaryOperations = { "not", "-" };

        
        public static bool IsOperator(this string @operator)
        {
            return Combiners.Any(x => String.Equals(x, @operator, StringComparison.OrdinalIgnoreCase))
                   || Operations.Any(x => String.Equals(x, @operator, StringComparison.OrdinalIgnoreCase))
                   || Arithmetic.Any(x => String.Equals(x, @operator, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsCombinationOperator(this string @operator)
        {
            return Combiners.Any(x => String.Equals(x, @operator, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsUnaryOperator(this string @operator)
        {
            return UnaryOperations.Any(x => string.Equals(x, @operator, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsLogicalOperator(this string @operator)
        {
            return Operations.Any(x => string.Equals(x, @operator, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsArithmeticOperator(this string @operator)
        {
            return Arithmetic.Any(x => string.Equals(x, @operator, StringComparison.OrdinalIgnoreCase));
        }

        public static ExpressionType GetExpressionType(this string @operator)
        {
            switch (@operator.ToLowerInvariant())
            {
                case "add":
                    return ExpressionType.Add;
                case "sub":
                    return ExpressionType.Subtract;

                case "mul":
                    return ExpressionType.Multiply;
                case "div":
                    return ExpressionType.Divide;
                case "mod":
                    return ExpressionType.Modulo;

                case "not":
                    return ExpressionType.Not;

                case "and":
                    return ExpressionType.AndAlso;
                case "or":
                    return ExpressionType.OrElse;

                case "gt":
                    return ExpressionType.GreaterThan;
                case "ge":
                    return ExpressionType.GreaterThanOrEqual;

                case "lt":
                    return ExpressionType.LessThan;
                case "le":
                    return ExpressionType.LessThanOrEqual;

                case "eq":
                    return ExpressionType.Equal;
                case "ne":
                    return ExpressionType.NotEqual;

                default:
                    throw new Exception();
            }
        }
    }
}
