using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestClass]
    public class AnyAllFunctionTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void AnyNoArgsTest()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any()), Filter.Deserialize<Customer>("Customers/Any()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any()), Filter.Deserialize<Customer>("EnumerableCustomers/Any()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any()), Filter.Deserialize<Customer>("ListCustomers/Any()")));
        }

        [TestMethod]
        public void Any()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any(x => true)), Filter.Deserialize<Customer>("Customers/Any(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/Any(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any(x => true)), Filter.Deserialize<Customer>("ListCustomers/Any(x: true)")));
        }

        [TestMethod]
        public void All()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.All(x => true)), Filter.Deserialize<Customer>("Customers/All(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.All(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/All(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.All(x => true)), Filter.Deserialize<Customer>("ListCustomers/All(x: true)")));
        }
    }
}
