using System.Linq;
using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    
    public class SerializeDateFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Day()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Day == 8)), "day(BirthDate) eq 8");            
        }

        [Fact]
        public void Hour()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Hour == 0)), "hour(BirthDate) eq 0");            
        }

        [Fact]
        public void Minute()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Minute == 0)), "minute(BirthDate) eq 0");            
        }

        [Fact]
        public void Month()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Month == 12)), "month(BirthDate) eq 12");            
        }

        [Fact]
        public void Second()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Second == 0)), "second(BirthDate) eq 0");            
        }

        [Fact]
        public void Year()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate.Year == 1948)), "year(BirthDate) eq 1948");            
        }

        [Fact]
        public void NullableDay()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Day == 8)), "day(NullableBirthDate/Value) eq 8");            
        }

        [Fact]
        public void NullableHour()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Hour == 0)), "hour(NullableBirthDate/Value) eq 0");            
        }

        [Fact]
        public void NullableMinute()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Minute == 0)), "minute(NullableBirthDate/Value) eq 0");            
        }

        [Fact]
        public void NullableMonth()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Month == 12)), "month(NullableBirthDate/Value) eq 12");            
        }

        [Fact]
        public void NullableSecond()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Second == 0)), "second(NullableBirthDate/Value) eq 0");            
        }

        [Fact]
        public void NullableYear()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableBirthDate.Value.Year == 1948)), "year(NullableBirthDate/Value) eq 1948");            
        }
    }
}
