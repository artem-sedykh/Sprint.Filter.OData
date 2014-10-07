using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using Sprint.Filter.Extensions;
using Sprint.Filter.OData.Serialize.Writers;

namespace Sprint.Filter.OData.Serialize
{
    internal class Translator
    {
        private static readonly IDictionary<MethodInfo, IMethodWriter> MethodWriters = new Dictionary<MethodInfo, IMethodWriter>();
        private readonly IDictionary<ParameterExpression, string> parameters = new Dictionary<ParameterExpression, string>();


        static Translator()
        {
            var queryableContains = typeof(Queryable).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 2);
            var enumerableContains = typeof(Enumerable).GetMethods().SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 2);
            var contains = new ContainsMethodWriter();

            MethodWriters[queryableContains] = contains;
            MethodWriters[enumerableContains] = contains;
        }

        public string VisitParameter(ParameterExpression expression)
        {
            return parameters[expression];
        }

        public string VisitMethodCall(MethodCallExpression expression)
        {

            if(expression.Method.IsGenericMethod)
            {
                var method = expression.Method.GetGenericMethodDefinition();

                if(MethodWriters.ContainsKey(method))
                    return MethodWriters[method].Write(expression, this);

                if(expression.Method.ReflectedType == typeof(Queryable) ||
                   expression.Method.ReflectedType == typeof(Enumerable))
                {
                    var writer = new QueryableMethodWriter();

                    return writer.Write(expression, this);
                }
                
            }


            return null;
        }

        public string VisitMember(MemberExpression memberExpr)
        {
            var isMemberOfParameter = IsMemberOfParameter(memberExpr);

            if(isMemberOfParameter)
            {
                var name = memberExpr.Member.Name;

                if (memberExpr.Expression != null)
                {
                    var left = Visit(memberExpr.Expression);

                    return String.IsNullOrWhiteSpace(left) ? name : String.Format("{0}/{1}", left, name);
                }

                return name;
            }

            var expr = CollapseCapturedOuterVariables(memberExpr);

            return Visit(expr);
        }

        public string VisitBinary(BinaryExpression expression)
        {
            var left = Visit(expression.Left);
            var right = Visit(expression.Right);

            return String.Format(expression.NodeType.ODataFormat(), left, right);            
        }

        public string VisitConstant(ConstantExpression constantExpr)
        {
            var type = constantExpr.Type;

            if(type == typeof(DateTime))
            {
                var dateTimeValue =(DateTime) constantExpr.Value;
                return String.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
            }


            return constantExpr.Value.ToString();
        }

        public string VisitLambda(LambdaExpression lambda, bool root)
        {
            if(root)
            {
                if (lambda.Parameters.Count > 1 && root)
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
                case ExpressionType.Call:
                    return VisitMethodCall((MethodCallExpression)expression);
                case ExpressionType.Constant:
                    return VisitConstant((ConstantExpression)expression);
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.NotEqual:
                case ExpressionType.Equal:
                    return VisitBinary((BinaryExpression)expression);
                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression, root);
                case ExpressionType.MemberAccess:
                    return VisitMember((MemberExpression)expression);
                case ExpressionType.Parameter:
                    return VisitParameter((ParameterExpression)expression);
                case ExpressionType.Not:
                case ExpressionType.Negate:
                    //return VisitUnary((ODataUnaryExpression)expression);

                    return null;
            }

            return null;
        }

        public string Translate(Expression expression)
        {
            var preTranslator = new PreExpressionTranslator();
            parameters.Clear();
            return Visit(preTranslator.Translate(expression), true);
        }


        private static Expression CollapseCapturedOuterVariables(MemberExpression input)
        {
            if(input == null || input.NodeType != ExpressionType.MemberAccess)
                return input;            
            
            if(input.Expression == null)
            {
                var val = GetValue(input);

                return Expression.Constant(val);
            }

            switch (input.Expression.NodeType)
            {
                case ExpressionType.New:
                case ExpressionType.MemberAccess:
                    var value = GetValue(input);
                    return Expression.Constant(value);
                case ExpressionType.Constant:
                    var obj = ((ConstantExpression)input.Expression).Value;
                    if (obj == null)
                    {
                        return input;
                    }

                    var fieldInfo = input.Member as FieldInfo;
                    if (fieldInfo != null)
                    {
                        var result = fieldInfo.GetValue(obj);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    var propertyInfo = input.Member as PropertyInfo;
                    if (propertyInfo != null)
                    {
                        var result = propertyInfo.GetValue(obj, null);
                        return result is Expression ? (Expression)result : Expression.Constant(result);
                    }

                    break;
                case ExpressionType.TypeAs:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return Expression.Constant(GetValue(input));
            }

            return input;
        }

        private static object GetValue(Expression input)
        {            
            var objectMember = Expression.Convert(input, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember).Compile();

            return getterLambda();
        }

        private static bool IsMemberOfParameter(MemberExpression input)
        {
            if (input == null || input.Expression == null)
            {
                return false;
            }

            var nodeType = input.Expression.NodeType;
            var tempExpression = input.Expression as MemberExpression;
            while (nodeType == ExpressionType.MemberAccess)
            {
                if (tempExpression == null || tempExpression.Expression == null)
                {
                    return false;
                }

                nodeType = tempExpression.Expression.NodeType;
                tempExpression = tempExpression.Expression as MemberExpression;
            }

            return nodeType == ExpressionType.Parameter;
        }

        public static bool ContainsParameters(Expression expression)
        {
            var visitor = new ParameterVisitor();
            visitor.Visit(expression);

            return visitor.ContainsParameters;
        }
    }

    internal class ParameterVisitor : ExpressionVisitor
    {
        public IList<ParameterExpression> parameters=new List<ParameterExpression>(); 
        public bool ContainsParameters { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ContainsParameters = true;

            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }
    }
}
