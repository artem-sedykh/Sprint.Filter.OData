using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestClass]
    public class DateFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void Day()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Day == 8), Filter.Deserialize<Customer>("day(BirthDate) eq 8")));
        }

        [TestMethod]
        public void Hour()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Hour == 0), Filter.Deserialize<Customer>("hour(BirthDate) eq 0")));
        }

        [TestMethod]
        public void Minute()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Minute == 0), Filter.Deserialize<Customer>("minute(BirthDate) eq 0")));
        }

        [TestMethod]
        public void Month()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Month == 12), Filter.Deserialize<Customer>("month(BirthDate) eq 12")));
        }

        [TestMethod]
        public void Second()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Second == 0), Filter.Deserialize<Customer>("second(BirthDate) eq 0")));
        }

        [TestMethod]
        public void Year()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Year == 1948), Filter.Deserialize<Customer>("year(BirthDate) eq 1948")));
        }

        [TestMethod]
        public void NullableDay()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Day == 8), Filter.Deserialize<Customer>("day(NullableBirthDate) eq 8")));
        }

        [TestMethod]
        public void NullableHour()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Hour == 0), Filter.Deserialize<Customer>("hour(NullableBirthDate) eq 0")));
        }

        [TestMethod]
        public void NullableMinute()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Minute == 0), Filter.Deserialize<Customer>("minute(NullableBirthDate) eq 0")));
        }

        [TestMethod]
        public void NullableMonth()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Month == 12), Filter.Deserialize<Customer>("month(NullableBirthDate) eq 12")));
        }

        [TestMethod]
        public void NullableSecond()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Second == 0), Filter.Deserialize<Customer>("second(NullableBirthDate) eq 0")));
        }

        [TestMethod]
        public void NullableYear()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Year == 1948), Filter.Deserialize<Customer>("year(NullableBirthDate) eq 1948")));
        }
    }
}
