using System;
using System.Linq.Expressions;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Common
{
    internal class ODataBinaryExpression : ODataExpression
    {
        public ODataBinaryExpression(ExpressionType type)
        {
            if(!type.IsBinary())
                throw new Exception();

            nodeType = type;
        }

        public ODataExpression Left { get; set; }

        public ODataExpression Right { get; set; }

        internal override string DebugView()
        {
            return string.Format(nodeType.DebugFormat(), Left, Right);
        }
    }
}
