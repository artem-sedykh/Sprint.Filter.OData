using System.Linq.Expressions;

namespace Sprint.Filter.Extensions
{
    internal static class ExpressionExtensions
    {
        public static bool IsNullConstant(this Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)expression;
                return constant.Value == null;
            }

            return false;
        }
    }
}
