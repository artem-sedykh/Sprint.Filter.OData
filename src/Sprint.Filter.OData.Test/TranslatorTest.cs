using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Common;
using Sprint.Filter.OData.Deserialize;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestClass]
    public class TranslatorTest
    {
        [TestMethod]
        public void LambdaExpression()
        {                       
            var tparameter = ODataExpression.Parameter("t");
            var cparameter = ODataExpression.Parameter("c");
            var l1 = new ODataLambdaExpression
            {
                Body = ODataExpression.PropertyOrField("Items", cparameter),
                Parameters = new[] { cparameter }
            };

            var aparameter = ODataExpression.Parameter("a");
            var bparameter = ODataExpression.Parameter("b");
            var l2= new ODataLambdaExpression
            {
                Body = ODataExpression.Add(ODataExpression.PropertyOrField("Id", aparameter), ODataExpression.PropertyOrField("Id", bparameter)),
                Parameters = new[] { aparameter,bparameter }
            };

            var method = new ODataMethodCallExpression
            {
                Context = ODataExpression.PropertyOrField("Customers", tparameter),
                MethodName = "SelectMany",
                Arguments = new ODataExpression[]
                {
                    l1,l2
                }
            };

            var expression = new ODataLambdaExpression
            {
                Body = method,
                Parameters = new [] { tparameter }
            };
            //t => t.Customers.SelectMany(c => c.Customers, (a, b) => (a.Id + b.Id))
            //t => t.Customers.SelectMany(c => c.Customers, (a, b) => (a.Id + b.Id))


            var methods =
                typeof(Queryable).GetMethods()
                    .Where(x => x.Name == "SelectMany" && x.GetParameters().Length == 3)
                    .ToArray();

            foreach(var methodInfo in methods)
            {
             //   var arguments = methodInfo.GetGenericArguments();
                
               // var parameter = DynamicExpression.Parameter(arguments[0], "t");
                
             //   var dd = DynamicExpression.Property(parameter, "Customers");
              //  

            }


            //var expr = Linq.Linq.Expr<Customer, IEnumerable<int>>(t => t.Customers.SelectMany(c => c.Items, (a, b) => a.Id + b.Id));
            //var translator = new QueryTranslator();

            //var d = translator.Invoke<Customer, IEnumerable<int>>(expression);
        }



        [TestMethod]
        public void Parce()
        {


            //Отзнозначное понимание выз
            //var str = "Customers/SelectMany<Sprint.Filter.OData.Test.Models.Customer,Sprint.Filter.OData.Test.Models.TestClass,System.Int32>(1,2,3)";

            //var expressionLexer = new ExpressionLexer(str);

            //var expr=expressionLexer.BuildLambdaExpression();
        }
    }
}
