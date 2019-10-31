using System;
using Xunit;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    public class DateFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        public DateFunctionsTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Day()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Day == 8), Filter.Deserialize<Customer>("day(BirthDate) eq 8")));
        }

        [Fact]
        public void Hour()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Hour == 0), Filter.Deserialize<Customer>("hour(BirthDate) eq 0")));
        }

        [Fact]
        public void Minute()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Minute == 0), Filter.Deserialize<Customer>("minute(BirthDate) eq 0")));
        }

        [Fact]
        public void Month()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Month == 12), Filter.Deserialize<Customer>("month(BirthDate) eq 12")));
        }

        [Fact]
        public void Second()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Second == 0), Filter.Deserialize<Customer>("second(BirthDate) eq 0")));
        }

        [Fact]
        public void Year()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Year == 1948), Filter.Deserialize<Customer>("year(BirthDate) eq 1948")));
        }

        [Fact]
        public void NullableDay()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Day == 8), Filter.Deserialize<Customer>("day(NullableBirthDate) eq 8")));
        }

        [Fact]
        public void NullableHour()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Hour == 0), Filter.Deserialize<Customer>("hour(NullableBirthDate) eq 0")));
        }

        [Fact]
        public void NullableMinute()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Minute == 0), Filter.Deserialize<Customer>("minute(NullableBirthDate) eq 0")));
        }

        [Fact]
        public void NullableMonth()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Month == 12), Filter.Deserialize<Customer>("month(NullableBirthDate) eq 12")));
        }

        [Fact]
        public void NullableSecond()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Second == 0), Filter.Deserialize<Customer>("second(NullableBirthDate) eq 0")));
        }

        [Fact]
        public void NullableYear()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Year == 1948), Filter.Deserialize<Customer>("year(NullableBirthDate) eq 1948")));
        }

        [Fact]
        public void DateTimePropertyAccess()
        {
            var expression = Linq.Linq.Expr<Customer, bool>(t => (DateTime.UtcNow.Date - t.BirthDate.Date).TotalDays > 1 || (DateTime.UtcNow.Date - t.BirthDate.Date).TotalDays > 1);

            var filter = Filter.Serialize(expression);

            var exp = Filter.Deserialize<Customer>(filter);

            Assert.True(ExpressionEqualityComparer.Equals(expression, exp));
        }

        [Fact]
        public void Now()
        {
            Assert.True(ExpressionEqualityComparer.Equals(
                Linq.Linq.Expr<Customer, bool>(t => t.BirthDate >= DateTime.Now),
                Filter.Deserialize<Customer>("BirthDate ge now()")));

        }

        [Fact]
        public void UtcNow()
        {
            Assert.True(ExpressionEqualityComparer.Equals(
                Linq.Linq.Expr<Customer, bool>(t => t.BirthDate >= DateTime.UtcNow.Date),
                Filter.Deserialize<Customer>("BirthDate ge utcnow()/Date")));
        }
    }
}
