using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sprint.Filter.OData;
using Sprint.Filter.OData.Test;
using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    
    public class CommonTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        public CommonTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void EnumTest()
        {
            //Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<TestClass, bool>(t => t.Status == Status.Completed),
            //    Filter.Deserialize<TestClass>("Status eq 2")));

            //Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<TestClass, bool>(t => t.NullableStatus == null),
            //    Filter.Deserialize<TestClass>("NullableStatus eq null")));

            //Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<TestClass, bool>(t => t.NullableStatus == Status.Pending),
            //    Filter.Deserialize<TestClass>("NullableStatus eq 0")));
        }

        [Fact]
        public void Isof()
        {
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Parent is Customer),
                Filter.Deserialize<Customer>("isof(Parent, Sprint.Filter.OData.Test.Models.Customer)")));

            // ReSharper disable once CSharpWarnings::CS0183
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t is Customer),
                 Filter.Deserialize<Customer>("isof(Sprint.Filter.OData.Test.Models.Customer)")));            
        }

        [Fact]
        public void Cast()
        {
            // ReSharper disable once RedundantCast
            var left = Linq.Linq.Expr<Customer, bool>(t => t.Parent as Customer != null);
            var right = Filter.Deserialize<Customer>("cast(Parent, Sprint.Filter.OData.Test.Models.Customer) ne null");
            Assert.True(ExpressionEqualityComparer.Equals(left, right));

            // ReSharper disable once RedundantCast
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t as Customer!=null),
                 Filter.Deserialize<Customer>("cast(Sprint.Filter.OData.Test.Models.Customer) ne null")));
        }

        [Fact]
        public void Test()
        {
         // //  var epxression = Linq.Linq.Expr<Customer, int>(x => x.Id>);

         //   var p = Expression.Parameter(typeof(Customer), "model");
         ////   var ddddd = Expression.Invoke(epxression, p);


         //   var visitor = new ExpandInvokeVistor();

         //   var dddd=visitor.Visit(ddddd);
            
         //   var t = Sprint.Filter.Helpers.Evaluator.PartialEval(Linq.Linq.Expr<Customer, bool>(x => x.BirthDate == Now()));
         //   var test = Linq.Linq.Expr<Customer, bool>(x => x.BirthDate == DateTime.Now);
        }

       // [ReturnConstantValueFunction]
        public static DateTime Now( )
        {
            return DateTime.Now;
        }
    }
    
    public class ExpandInvokeVistor : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> parameters = new Dictionary<ParameterExpression, ParameterExpression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return parameters.ContainsKey(node) ? parameters[node] : base.VisitParameter(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            var lambda = (LambdaExpression)node.Expression;

            parameters = node.Arguments.Select((e, i) => new
            {
                Parameter = e,
                ReplaceParameter = lambda.Parameters[i]
            }).ToDictionary(x => x.ReplaceParameter, x => (ParameterExpression)x.Parameter);

            return base.Visit(node.Expression);
        }

        public override Expression Visit(Expression node)
        {            
            return base.Visit(node);
        }
    }
}
