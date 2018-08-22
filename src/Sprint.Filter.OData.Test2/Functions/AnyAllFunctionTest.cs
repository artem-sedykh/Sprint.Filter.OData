using System.Linq;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestFixture]
    public class AnyAllFunctionTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void AnyNoArgsTest()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any()), Filter.Deserialize<Customer>("Customers/Any()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any()), Filter.Deserialize<Customer>("EnumerableCustomers/Any()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any()), Filter.Deserialize<Customer>("ListCustomers/Any()")));
        }

        [Test]
        public void Any()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Any(x => true)), Filter.Deserialize<Customer>("Customers/Any(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.Any(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/Any(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.Any(x => true)), Filter.Deserialize<Customer>("ListCustomers/Any(x: true)")));
        }

        [Test]
        public void All()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.All(x => true)), Filter.Deserialize<Customer>("Customers/All(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumerableCustomers.All(x => true)), Filter.Deserialize<Customer>("EnumerableCustomers/All(x: true)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.ListCustomers.All(x => true)), Filter.Deserialize<Customer>("ListCustomers/All(x: true)")));
        }
    }
}
