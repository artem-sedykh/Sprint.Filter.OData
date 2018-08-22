using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataLambdaExpression : ODataExpression
    {
        public ODataLambdaExpression()
        {
            nodeType = ExpressionType.Lambda;
        }

        public ODataExpression Body { get; set; }

        public ODataParameterExpression[] Parameters { get; set; }

        internal override string DebugView()
        {
            if(Parameters.Length==0)
                return $"() => {Body}";

            if (Parameters.Length > 1)
                return $"({string.Join(", ", Parameters.Select(x => x.DebugView()))}) => {Body}";

            return $"{Parameters[0].DebugView()} => {Body?.DebugView()}";
        }
    }
}
