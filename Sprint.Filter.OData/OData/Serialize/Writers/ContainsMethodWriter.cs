using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Serialize.Writers
{
    internal class ContainsMethodWriter:IMethodWriter
    {
        public string Write(MethodCallExpression expression, Translator translator)
        {
            var source = expression.Arguments[0];
            var item = expression.Arguments[1];
            var identificator = translator.Visit(item);

            if (source.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)source;

                var values = (from object obj in (IEnumerable)constant.Value
                              select String.Format("{0} eq {1}", identificator, translator.VisitConstant(Expression.Constant(obj)))).ToArray();

                return "(" + String.Join(" or ", values) + ")";
            }

            return string.Format("{0}/{1}({2})", translator.Visit(source), expression.Method.Name, identificator);
        }
    }
}
