using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sprint.Filter.Extensions;
using Sprint.Filter.OData.Serialize.Writers;

namespace Sprint.Filter.OData.Serialize
{
    internal class Translator
    {
        #region Writers

        private static readonly IValueWriter[] ValueWriters = new IValueWriter[]
        {
            new BooleanValueWriter(),
            new ByteArrayValueWriter(),
            new ByteValueWriter(), 
            new DateTimeOffsetValueWriter(), 
            new DateTimeValueWriter(), 
            new DecimalValueWriter(), 
            new SingleValueWriter(), 
            new DoubleValueWriter(), 
            new EnumValueWriter(), 
            new GuidValueWriter(), 
            new StreamValueWriter(), 
            new StringValueWriter(), 
            new TimeSpanValueWriter(), 
            new IntegerValueWriter<int>(),
            new IntegerValueWriter<long>(),
            new IntegerValueWriter<short>(),
            new IntegerValueWriter<uint>(),
            new IntegerValueWriter<ulong>(),
            new IntegerValueWriter<ushort>()            
        };

        private static readonly IMethodWriter DefaultMethodWriter = null;

        private static readonly IMethodWriter[] MethodWriters = new IMethodWriter[]
        {
            new ContainsMethodWriter(),
            new QueryableMethodWriter(),

            new MathCeilingMethodWriter(), 
            new MathFloorMethodWriter(), 
            new MathRoundMethodWriter(),

            new StringContainsMethodWriter(), 
            new StringEndsWithMethodWriter(), 
            new StringIndexOfMethodWriter(), 
            new StringReplaceMethodWriter(), 
            new StringStartsWithMethodWriter(), 
            new StringSubstringMethodWriter(), 
            new StringToLowerMethodWriter(), 
            new StringToUpperMethodWriter(), 
            new StringTrimMethodWriter()
        };

        #endregion

        private readonly IDictionary<ParameterExpression, string> parameters = new Dictionary<ParameterExpression, string>();

        private static readonly ExpressionPreparator Preparator = new ExpressionPreparator();

        public string VisitParameter(ParameterExpression expression)
        {
            return parameters[expression];
        }

        public string VisitMethodCall(MethodCallExpression expression)
        {
            var writer = MethodWriters.FirstOrDefault(w => w.CanHandle(expression));

            if(writer != null)
                return writer.Write(expression, e => Visit(e));

            //TODO: Добавть userFunctions methodWriter;

            return DefaultMethodWriter.Write(expression, e => Visit(e));
        }

        public string VisitMemberAccess(MemberExpression memberExpr)
        {
            var name = memberExpr.Member.Name;

            if (memberExpr.Expression != null)
            {
                var left = Visit(memberExpr.Expression);

                return String.IsNullOrWhiteSpace(left) ? name : String.Format("{0}/{1}", left, name);
            }

            return name;
        }

        public string VisitBinary(BinaryExpression expression)
        {
            var left = Visit(expression.Left);
            var right = Visit(expression.Right);

            return String.Format(expression.NodeType.ODataFormat(), left, right);            
        }

        public string VisitConstant(ConstantExpression constantExpr)
        {
            var type = Nullable.GetUnderlyingType(constantExpr.Type) ?? constantExpr.Type;

            if(constantExpr.Value == null)
                return "null";

            var writer = ValueWriters.FirstOrDefault(w => w.Handles(type));

            if(writer == null)
                throw new NotSupportedException(String.Format("type '{0}' is not supported", type));

            return writer.Write(constantExpr.Value);            
        }

        public string VisitLambda(LambdaExpression lambda, bool root)
        {
            if(root)
            {
                if (lambda.Parameters.Count > 1)
                    throw new NotSupportedException();

                parameters[lambda.Parameters[0]] = String.Empty;

                return Visit(lambda.Body);   
            }

            foreach (var p in lambda.Parameters)            
                parameters[p] = p.Name;

            return lambda.Parameters.Count > 1 
                ? String.Format("({0}): {1}", String.Join(", ", lambda.Parameters.Select(x => x.Name)),Visit(lambda.Body))
                : String.Format("{0}: {1}", lambda.Parameters[0].Name, Visit(lambda.Body));
        }

        internal string VisitQuote(UnaryExpression expression)
        {
            return Visit(expression.Operand);
        }

        internal string Visit(Expression expression, bool root=false)
        {
            if (expression == null)
                return null;

            switch (expression.NodeType)
            {
                case ExpressionType.Quote:
                    return VisitQuote((UnaryExpression)expression);
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.ArrayLength:
                case ExpressionType.TypeAs:
                    throw new NotSupportedException(expression.ToString());//return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.ArrayIndex:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                    return VisitBinary((BinaryExpression)expression);
                case ExpressionType.TypeIs:
                    //return this.VisitTypeIs((TypeBinaryExpression)exp);
                case ExpressionType.Conditional:
                    throw new NotSupportedException(expression.ToString());
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)expression);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)expression);
                case ExpressionType.MemberAccess:
                    return VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)expression);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression, root);
                case ExpressionType.New:
                    throw new NotSupportedException(expression.ToString());
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                    //return this.VisitNewArray((NewArrayExpression)exp);
                case ExpressionType.Invoke:
                    throw new NotSupportedException(expression.ToString());//return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.MemberInit:
                    throw new NotSupportedException(expression.ToString());//return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.ListInit:
                    throw new NotSupportedException(expression.ToString());//return this.VisitListInit((ListInitExpression)exp);
                default:
                    throw new Exception(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }
        }

        public string Translate(Expression expression)
        {            
            parameters.Clear();

            return Visit(Preparator.Visit(expression), true);
        }                      
    }    
}
