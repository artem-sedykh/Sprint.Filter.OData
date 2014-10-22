using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprint.Filter.OData.Serialize;

namespace Sprint.Filter.OData.Test.QueryProviderTests
{
    public class CustomQueryProvider:QueryProvider
    {
        //public override string GetQueryText(Expression expression)
        //{

        //    var methodCallExpression = (MethodCallExpression)expression;
        //    var source = methodCallExpression.Arguments[0]; //Исходные данные 
        //    var argument = methodCallExpression.Arguments[1];

        //    if (expression.NodeType == ExpressionType.Call)
        //    {
        //        var translator = new Translator();
        //        var expr = (MethodCallExpression)expression;

        //        var res= String.Format("$filter={0}", translator.Translate(expr.Arguments[1]));

        //        return res;
        //    }

        //    return null;
        //}       

        public CustomQueryProvider(Uri address) : base(address)
        {
        }

        public override object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
