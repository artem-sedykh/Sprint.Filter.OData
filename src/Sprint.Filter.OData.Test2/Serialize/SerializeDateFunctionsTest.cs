using System.Linq;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeDateFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void Day()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Day == 8)), "day(BirthDate) eq 8");            
        }

        [Test]
        public void Hour()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Hour == 0)), "hour(BirthDate) eq 0");            
        }

        [Test]
        public void Minute()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Minute == 0)), "minute(BirthDate) eq 0");            
        }

        [Test]
        public void Month()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Month == 12)), "month(BirthDate) eq 12");            
        }

        [Test]
        public void Second()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Second == 0)), "second(BirthDate) eq 0");            
        }

        [Test]
        public void Year()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Year == 1948)), "year(BirthDate) eq 1948");            
        }

        [Test]
        public void NullableDay()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Day == 8)), "day(NullableBirthDate/Value) eq 8");            
        }

        [Test]
        public void NullableHour()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Hour == 0)), "hour(NullableBirthDate/Value) eq 0");            
        }

        [Test]
        public void NullableMinute()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Minute == 0)), "minute(NullableBirthDate/Value) eq 0");            
        }

        [Test]
        public void NullableMonth()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Month == 12)), "month(NullableBirthDate/Value) eq 12");            
        }

        [Test]
        public void NullableSecond()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Second == 0)), "second(NullableBirthDate/Value) eq 0");            
        }

        [Test]
        public void NullableYear()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Year == 1948)), "year(NullableBirthDate/Value) eq 1948");            
        }
    }
}
