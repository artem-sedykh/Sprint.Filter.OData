using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestFixture]
    public class PrecedenceGroupingTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void Brackets()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Filter.Deserialize<Customer>("Id mul (3 add Id) ge -15"), Linq.Linq.Expr<Customer, bool>(t => t.Id * (3 + t.Id) >= -15)));
        }
    }
}
