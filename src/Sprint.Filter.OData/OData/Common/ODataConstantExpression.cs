using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Common
{
    internal sealed class ODataConstantExpression : ODataExpression
    {
        private object _value { get; set; }
        private Type _type { get; set; }

        public ODataConstantExpression(object value)
        {
            _type = value.GetType();

            _value = value;

            nodeType = ExpressionType.Constant;
        }
        public ODataConstantExpression(object value, Type type)
        {
            _value = value;

            _type = type;

            if(value!=null && value.GetType()!=type)
                throw new Exception("Types are not equal");

            nodeType = ExpressionType.Constant;
        }

        public Type Type => _type;

        public object Value => _value;

        internal override string DebugView()
        {
            return string.Format(_type == typeof(string) ? "\"{0}\"" : "{0}", _value?.ToString() ?? "null");
        }
    }
}
