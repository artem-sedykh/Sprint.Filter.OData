using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeCustomPropertyName
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void CustomName()
        {
            var query = Filter.Serialize<Customer>(t => t.CustomName1 == 1 || t.CustomName2 == 2 || t.CustomName3 == 3);

            Assert.AreEqual(query, "(cn1 eq 1 or cn2 eq 2) or cn3 eq 3");
        }
    }
}
