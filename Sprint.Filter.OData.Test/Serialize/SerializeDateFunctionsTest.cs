using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestClass]
    public class SerializeDateFunctionsTest
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
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Day == 8)), "day(BirthDate) eq 8");            
        }

        [TestMethod]
        public void Hour()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Hour == 0)), "hour(BirthDate) eq 0");            
        }

        [TestMethod]
        public void Minute()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Minute == 0)), "minute(BirthDate) eq 0");            
        }

        [TestMethod]
        public void Month()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Month == 12)), "month(BirthDate) eq 12");            
        }

        [TestMethod]
        public void Second()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Second == 0)), "second(BirthDate) eq 0");            
        }

        [TestMethod]
        public void Year()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.BirthDate.Year == 1948)), "year(BirthDate) eq 1948");            
        }

        [TestMethod]
        public void NullableDay()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Day == 8)), "day(NullableBirthDate/Value) eq 8");            
        }

        [TestMethod]
        public void NullableHour()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Hour == 0)), "hour(NullableBirthDate/Value) eq 0");            
        }

        [TestMethod]
        public void NullableMinute()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Minute == 0)), "minute(NullableBirthDate/Value) eq 0");            
        }

        [TestMethod]
        public void NullableMonth()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Month == 12)), "month(NullableBirthDate/Value) eq 12");            
        }

        [TestMethod]
        public void NullableSecond()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Second == 0)), "second(NullableBirthDate/Value) eq 0");            
        }

        [TestMethod]
        public void NullableYear()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Year == 1948)), "year(NullableBirthDate/Value) eq 1948");            
        }
    }
}
