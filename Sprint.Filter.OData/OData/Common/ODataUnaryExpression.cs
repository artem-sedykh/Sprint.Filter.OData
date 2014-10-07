using System;
using System.Linq.Expressions;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataUnaryExpression : ODataExpression
    {        
        public ODataUnaryExpression(ExpressionType type)
        {
            nodeType = type;
        }       

        public ODataExpression Operand { get; set; }        

        public Type Type
        {
            get { return typeof(Boolean); }
        }

        internal override string DebugView()
        {
            return String.Format(nodeType.DebugFormat(), Operand);
        }        
    }
}
