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

        private static readonly IValueWriter[] ValueWriters =
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

        private static readonly IMethodWriter DefaultMethodWriter = new DefaultMethodWriter();

        private static readonly IMethodWriter UserFunctionMethodWriter = new UserFunctionMethodWriter();

        private static readonly IMethodWriter[] MethodWriters =
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

        private static readonly Type DateTimeType = typeof(DateTime);

        private static readonly Type StringType = typeof(string);

        private readonly IMemberNameProvider memberNameProvider = new MemberNameProvider();

        public string VisitParameter(ParameterExpression expression)
        {
            return parameters[expression];
        }

        public string VisitMethodCall(MethodCallExpression expression)
        {
            var writer = MethodWriters.FirstOrDefault(w => w.CanHandle(expression));

            if(writer != null)
                return writer.Write(expression, e => Visit(e));

            if (UserFunctionMethodWriter.CanHandle(expression))
                return UserFunctionMethodWriter.Write(expression, e => Visit(e));

            return DefaultMethodWriter.Write(expression, e => Visit(e));
        }

        public string VisitMemberAccess(MemberExpression memberExpr)
        {
            var memberCall = GetMemberCall(memberExpr);
            
            if(memberCall != null)            
                return String.Format("{0}({1})", memberCall, Visit(memberExpr.Expression));

            var name = memberNameProvider.ResolveName(memberExpr.Member);
          
            if (memberExpr.Expression != null)
            {                
                var left = Visit(memberExpr.Expression);

                return String.IsNullOrWhiteSpace(left) ? name : String.Format("{0}/{1}", left, name);
            }

            return name;
        }

        private static string GetMemberCall(MemberExpression memberExpression)
        {
            var declaringType = memberExpression.Member.DeclaringType;
            var name = memberExpression.Member.Name;

            if (declaringType == StringType && String.Equals(name, "Length"))            
                return name.ToLowerInvariant();

            if (declaringType == DateTimeType)
            {
                switch (name)
                {
                    case "Hour":
                    case "Minute":
                    case "Second":
                    case "Day":
                    case "Month":
                    case "Year":
                        return name.ToLowerInvariant();
                }
            }

            return null;
        }

        public string VisitBinary(BinaryExpression expression)
        {                       
            var left = Visit(expression.Left);
            var right = Visit(expression.Right);

            return String.Format(expression.Method == MethodProvider.ConcatMethod ? "concat({0}, {1})" : expression.NodeType.ODataFormat(), left, right);
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

        internal string VisitConvert(UnaryExpression expression)
        {
            if (expression.Type.IsValueType)
                return Visit(expression.Operand);
            
            throw new NotSupportedException(expression.ToString());
        }

        internal string VisitNegate(UnaryExpression expression)
        {
            return "-" + Visit(expression.Operand);
        }

        internal string VisitNot(UnaryExpression expression)
        {
            if (expression.Operand.NodeType.IsBinary() || expression.Operand.NodeType.IsBinaryLogical())
                return String.Format("not ({0})", Visit(expression.Operand));

            return String.Format("not {0}", Visit(expression.Operand));
        }

        internal string VisitArrayLength(UnaryExpression expression)
        {
            return String.Format("{0}/Length", Visit(expression.Operand));            
        }

        internal string VisitTypeIs(TypeBinaryExpression expression)
        {
            var member = Visit(expression.Expression);

            return String.IsNullOrEmpty(member) ? String.Format("isof({0})", expression.TypeOperand.FullName) : String.Format("isof({0}, {1})", member, expression.TypeOperand.FullName);
        }

        internal string VisitTypeAs(UnaryExpression expression)
        {
            var operand = Visit(expression.Operand);

            return String.IsNullOrEmpty(operand) ? String.Format("cast({0})", expression.Type.FullName) : String.Format("cast({0}, {1})",operand, expression.Type.FullName);
        }

        internal string Visit(Expression expression, bool root=false)
        {
            if (expression == null)
                return null;

            switch (expression.NodeType)
            {
                case ExpressionType.ArrayLength:
                    return VisitArrayLength((UnaryExpression)expression); 
                case ExpressionType.Not:
                    return VisitNot((UnaryExpression)expression);
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked: 
                    return VisitNegate((UnaryExpression)expression);
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return VisitConvert((UnaryExpression)expression);
                case ExpressionType.Quote:
                    return VisitQuote((UnaryExpression)expression);                                                                           
                case ExpressionType.TypeAs:
                    return VisitTypeAs((UnaryExpression)expression);                    
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
                    return VisitTypeIs((TypeBinaryExpression)expression);                                    
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
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:                    
                case ExpressionType.Invoke:                    
                case ExpressionType.MemberInit:                    
                case ExpressionType.ListInit:
                case ExpressionType.Conditional:
                    throw new NotSupportedException(expression.ToString());

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
