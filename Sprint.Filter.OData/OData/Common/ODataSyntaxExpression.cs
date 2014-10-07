using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataSyntaxExpression : ODataExpression
    {
        private readonly char _syntax;

        public ODataSyntaxExpression(char syntax)
        {
            nodeType = ExpressionType.Default;

            _syntax = syntax;
        }

        public char Syntax
        {
            get { return _syntax; }            
        }

        internal override string DebugView()
        {
            return String.Format("'{0}'", Syntax);
        }        
    }
}
