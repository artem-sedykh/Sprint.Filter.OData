using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataLogicalBinaryExpression : ODataBinaryExpression
    {
        public ODataLogicalBinaryExpression(ExpressionType type) : base(type){}       
    }
}
