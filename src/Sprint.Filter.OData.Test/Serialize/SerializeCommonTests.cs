using System;
using System.Linq;
using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    
    public class SerializeCommonTests
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void ArrayLength()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.CustomersArray.Length == 15)), "CustomersArray/Length eq 15");            
        }

        [Fact]
        public void Isof()
        {            
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Parent is Customer)), "isof(Parent, Sprint.Filter.OData.Test.Models.Customer)");
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t is Customer)), "isof(Sprint.Filter.OData.Test.Models.Customer)");
        }


        [Fact]
        public void Cast()
        {            
            // ReSharper disable once RedundantCast
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Parent as Customer !=null)), "cast(Parent, Sprint.Filter.OData.Test.Models.Customer) ne null");

            // ReSharper disable once RedundantCast
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t as Customer != null)), "cast(Sprint.Filter.OData.Test.Models.Customer) ne null");
        }

        [Fact]
        public void Select()
        {            
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => x.Id).Any())), "Customers/Select(x: x/Id)/Any()");
        }

        [Fact]
        public void SelectNew()
        {
            Assert.Throws<NotSupportedException>(() => Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => new {x.Id}).Any())));
        }

        [Fact]
        public void Property()
        {
            var expr = Linq.Linq.Expr<Customer, int>(t => t.Items.First().Id);

            var query = Filter.Serialize(expr);

            Assert.False(string.IsNullOrWhiteSpace(query));

            var expression = Filter.Deserialize(typeof(Customer), query);

            Assert.NotNull(expression);
        }
    }
}
