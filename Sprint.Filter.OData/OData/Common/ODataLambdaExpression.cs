using System;
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
                return String.Format("() => {0}", Body);

            if (Parameters.Length > 1)
                return String.Format("({0}) => {1}", String.Join(", ", Parameters.Select(x => x.DebugView())), Body);

            return String.Format("{0} => {1}", Parameters[0].DebugView(), Body!=null?Body.DebugView():null);
        }
    }
}
