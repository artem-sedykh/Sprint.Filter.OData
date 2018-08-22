using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataSyntaxExpression : ODataExpression
    {
        public ODataSyntaxExpression(char syntax)
        {
            nodeType = ExpressionType.Default;

            Syntax = syntax;
        }

        public char Syntax { get; }

        internal override string DebugView()
        {
            return $"'{Syntax}'";
        }
    }
}
