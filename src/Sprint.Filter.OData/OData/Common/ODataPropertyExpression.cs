using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataPropertyExpression:ODataExpression
    {
        public ODataPropertyExpression()
        {
            nodeType = ExpressionType.MemberAccess;
        }
        public string Name { get; set; }

        public ODataExpression Expression { get; set; }

        internal override string DebugView()
        {
            return Expression != null ? $"{Expression}.{Name}" : Name;
        }
    }
}
