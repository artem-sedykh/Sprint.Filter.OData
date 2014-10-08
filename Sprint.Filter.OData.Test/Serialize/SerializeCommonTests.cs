using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestClass]
    public class SerializeCommonTests
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void ArrayLength()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.CustomersArray.Length == 15)), "CustomersArray/Length eq 15");            
        }

        [TestMethod]
        public void Isof()
        {            
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Parent is Customer)), "isof(Parent, Sprint.Filter.OData.Test.Models.Customer)");
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t is Customer)), "isof(Sprint.Filter.OData.Test.Models.Customer)");
        }


        [TestMethod]
        public void Cast()
        {            
            // ReSharper disable once RedundantCast
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Parent as Customer !=null)), "cast(Parent, Sprint.Filter.OData.Test.Models.Customer) ne null");

            // ReSharper disable once RedundantCast
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t as Customer != null)), "cast(Sprint.Filter.OData.Test.Models.Customer) ne null");
        }
    }
}
