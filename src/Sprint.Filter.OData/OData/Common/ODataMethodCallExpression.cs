using System.Linq;
using System.Linq.Expressions;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataMethodCallExpression : ODataExpression
    {
        public ODataMethodCallExpression()
        {
            nodeType = ExpressionType.Call;

            Arguments = new ODataExpression[0];
        }

        public ODataExpression[] Arguments { get; set; }

        public string MethodName { get; set; }

        public ODataExpression Context { get; set; }

        internal override string DebugView()
        {
            var arguments = string.Join(", ", Arguments.Select(t => t.ToString()));

            return Context != null
                ? $"{Context}.{MethodName.Capitalize()}({arguments})"
                : $"{MethodName.Capitalize()}({arguments})";
        }
    }
}
