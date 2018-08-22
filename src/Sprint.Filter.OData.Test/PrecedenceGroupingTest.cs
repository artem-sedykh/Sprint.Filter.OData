using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestClass]
    public class PrecedenceGroupingTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void Brackets()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Filter.Deserialize<Customer>("Id mul (3 add Id) ge -15"), Linq.Linq.Expr<Customer, bool>(t => t.Id * (3 + t.Id) >= -15)));
        }
    }
}
