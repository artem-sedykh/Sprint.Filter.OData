using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataParameterExpression : ODataExpression
    {
        public ODataParameterExpression()
        {
            nodeType = ExpressionType.Parameter;
        }
        public string Name { get; set; }

        internal override string DebugView()
        {
            return Name;
        }
    }
}
