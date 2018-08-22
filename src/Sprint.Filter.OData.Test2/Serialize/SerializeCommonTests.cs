using System;
using System.Linq;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeCommonTests
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void ArrayLength()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.CustomersArray.Length == 15)), "CustomersArray/Length eq 15");            
        }

        [Test]
        public void Isof()
        {            
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Parent is Customer)), "isof(Parent, Sprint.Filter.OData.Test.Models.Customer)");
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t is Customer)), "isof(Sprint.Filter.OData.Test.Models.Customer)");
        }


        [Test]
        public void Cast()
        {            
            // ReSharper disable once RedundantCast
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Parent as Customer !=null)), "cast(Parent, Sprint.Filter.OData.Test.Models.Customer) ne null");

            // ReSharper disable once RedundantCast
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t as Customer != null)), "cast(Sprint.Filter.OData.Test.Models.Customer) ne null");
        }

        [Test]
        public void Select()
        {            
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => x.Id).Any())), "Customers/Select(x: x/Id)/Any()");
        }

        [Test]
        public void SelectNew()
        {
            Assert.That(() => Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => new { x.Id }).Any())), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void Property()
        {
            var expr = Linq.Linq.Expr<Customer, int>(t => t.Items.First().Id);

            var query = Filter.Serialize(expr);

            Assert.IsFalse(string.IsNullOrWhiteSpace(query));

            var expression = Filter.Deserialize(typeof(Customer), query);

            Assert.IsNotNull(expression);
        }
    }
}
