using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestClass]
    public class CommonTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void Isof()
        {
            // ReSharper disable once CSharpWarnings::CS0183
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Parent is Customer),
                Filter.Deserialize<Customer>("isof(Parent, Sprint.Filter.OData.Test.Models.Customer)")));

            // ReSharper disable once CSharpWarnings::CS0183
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t is Customer),
                 Filter.Deserialize<Customer>("isof(Sprint.Filter.OData.Test.Models.Customer)")));            
        }

        [TestMethod]
        public void Cast()
        {
            // ReSharper disable once RedundantCast
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Parent as Customer!=null),
                Filter.Deserialize<Customer>("cast(Parent, Sprint.Filter.OData.Test.Models.Customer) ne null")));

            // ReSharper disable once RedundantCast
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t as Customer!=null),
                 Filter.Deserialize<Customer>("cast(Sprint.Filter.OData.Test.Models.Customer) ne null")));
        }
    }
}
