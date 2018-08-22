using System;
using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;


namespace Sprint.Filter.OData.Test.Functions
{
    
    public class MathFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public MathFunctionsTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Round()
        {            
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimal) == 32), Filter.Deserialize<Customer>("round(FreightDecimal) eq 32")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDouble) == 32), Filter.Deserialize<Customer>("round(FreightDouble) eq 32")));
        }

        [Fact]
        public void RoundNullable()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimalNullable.Value) == 32), Filter.Deserialize<Customer>("round(FreightDecimalNullable) eq 32")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDoubleNullable.Value) == 32), Filter.Deserialize<Customer>("round(FreightDoubleNullable) eq 32")));
        }

        [Fact]
        public void Ceiling()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimal) == 33), Filter.Deserialize<Customer>("ceiling(FreightDecimal) eq 33")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDouble) == 33), Filter.Deserialize<Customer>("ceiling(FreightDouble) eq 33")));
        }

        [Fact]
        public void CeilingNullable()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimalNullable.Value) == 33), Filter.Deserialize<Customer>("ceiling(FreightDecimalNullable) eq 33")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDoubleNullable.Value) == 33), Filter.Deserialize<Customer>("ceiling(FreightDoubleNullable) eq 33")));
        }

        [Fact]
        public void Floor()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimal) == 34), Filter.Deserialize<Customer>("floor(FreightDecimal) eq 34")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDouble) == 34), Filter.Deserialize<Customer>("floor(FreightDouble) eq 34")));
        }

        [Fact]
        public void FloorNullable()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimalNullable.Value) == 34), Filter.Deserialize<Customer>("floor(FreightDecimalNullable) eq 34")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDoubleNullable.Value) == 34), Filter.Deserialize<Customer>("floor(FreightDoubleNullable) eq 34")));
        }
    }
}
