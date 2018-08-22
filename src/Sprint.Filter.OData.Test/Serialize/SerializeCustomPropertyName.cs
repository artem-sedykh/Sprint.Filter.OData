using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestClass]
    public class SerializeCustomPropertyName
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void CustomName()
        {
            var query = Filter.Serialize<Customer>(t => t.CustomName1 == 1 || t.CustomName2 == 2 || t.CustomName3 == 3);

            Assert.AreEqual(query, "cn1 eq 1 or cn2 eq 2 or cn3 eq 3");
        }
    }
}
