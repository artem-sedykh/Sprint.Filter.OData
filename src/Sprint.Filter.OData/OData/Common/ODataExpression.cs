using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Common
{
    [DebuggerDisplay("{DebugView()}")]
    internal abstract class ODataExpression
    {        
        protected ExpressionType nodeType;

        public virtual ExpressionType NodeType
        {
            get { return nodeType; }
        }

        internal virtual string DebugView()
        {
            return ToString();
        }

        public override string ToString()
        {
#if DEBUG
            return DebugView();
#endif
            return base.ToString();
        }

        public static ODataUnaryExpression MakeUnary(ExpressionType nodeType, ODataExpression operand)
        {
            return new ODataUnaryExpression(nodeType) { Operand = operand };
        }

        public static ODataConstantExpression Constant(object value)
        {
            return new ODataConstantExpression(value);
        }

        public static ODataConstantExpression Constant(object value, Type type)
        {
            return new ODataConstantExpression(value, type);
        }

        public static ODataPropertyExpression PropertyOrField(string name,ODataExpression parent)
        {
            return new ODataPropertyExpression { Name = name, Expression = parent };
        }

        public static ODataPropertyExpression PropertyOrField(string name)
        {
            return new ODataPropertyExpression { Name = name };
        }

        public static ODataParameterExpression Parameter(string name)
        {
            return new ODataParameterExpression { Name = name };
        }

        public static ODataBinaryExpression MakeBinary(ExpressionType nodeType, ODataExpression left,
            ODataExpression right)
        {
            return nodeType.IsBinaryLogical() 
                ? new ODataLogicalBinaryExpression(nodeType){Left = left,Right = right} 
                : new ODataBinaryExpression(nodeType) { Left = left, Right = right };
        }

        #region Logical

        public static ODataBinaryExpression GreaterThan(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.GreaterThan) { Left = left, Right = right };
        }

        public static ODataBinaryExpression GreaterThanOrEqual(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.GreaterThanOrEqual) { Left = left, Right = right };
        }

        public static ODataBinaryExpression LessThanOrEqual(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.LessThanOrEqual) { Left = left, Right = right };
        }

        public static ODataBinaryExpression LessThan(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.LessThan) { Left = left, Right = right };
        }

        public static ODataBinaryExpression Equal(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.Equal) { Left = left, Right = right };
        }

        public static ODataBinaryExpression NotEqual(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.NotEqual) { Left = left, Right = right };
        }

        public static ODataBinaryExpression OrElse(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.OrElse) { Left = left, Right = right };
        }

        public static ODataBinaryExpression AndAlso(ODataExpression left, ODataExpression right)
        {
            return new ODataLogicalBinaryExpression(ExpressionType.AndAlso) { Left = left, Right = right };
        }

        public static ODataUnaryExpression Not(ODataExpression operand)
        {
            return new ODataUnaryExpression(ExpressionType.Not) { Operand = operand };
        }

        #endregion

        #region Arithmetic

        public static ODataExpression Add(ODataExpression left, ODataExpression right)
        {
            return new ODataBinaryExpression(ExpressionType.Add) { Left = left, Right = right };
        }

        public static ODataExpression Subtract(ODataExpression left, ODataExpression right)
        {
            return new ODataBinaryExpression(ExpressionType.Subtract) { Left = left, Right = right };
        }

        public static ODataExpression Multiply(ODataExpression left, ODataExpression right)
        {
            return new ODataBinaryExpression(ExpressionType.Multiply) { Left = left, Right = right };
        }

        public static ODataExpression Divide(ODataExpression left, ODataExpression right)
        {
            return new ODataBinaryExpression(ExpressionType.Divide) { Left = left, Right = right };
        }

        public static ODataExpression Modulo(ODataExpression left, ODataExpression right)
        {
            return new ODataBinaryExpression(ExpressionType.Modulo) { Left = left, Right = right };
        }

        #endregion
    }
}
