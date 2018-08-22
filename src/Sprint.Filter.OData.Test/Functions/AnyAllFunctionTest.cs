using System.Linq;
using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    
    public class AnyAllFunctionTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public AnyAllFunctionTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void AnyNoArgsTest()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any()), Filter.Deserialize<Customer>("Customers/Any()")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any()), Filter.Deserialize<Customer>("EnumerableCustomers/Any()")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any()), Filter.Deserialize<Customer>("ListCustomers/Any()")));
        }

        [Fact]
        public void Any()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any(x => true)), Filter.Deserialize<Customer>("Customers/Any(x: true)")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/Any(x: true)")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any(x => true)), Filter.Deserialize<Customer>("ListCustomers/Any(x: true)")));
        }

        [Fact]
        public void All()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.All(x => true)), Filter.Deserialize<Customer>("Customers/All(x: true)")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.All(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/All(x: true)")));

            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.All(x => true)), Filter.Deserialize<Customer>("ListCustomers/All(x: true)")));
        }
    }
}
