using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;


namespace Sprint.Filter.OData.Test.Functions
{
    [TestClass]
    public class MathFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void Round()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimal) == 32), Filter.Deserialize<Customer>("round(FreightDecimal) eq 32")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDouble) == 32), Filter.Deserialize<Customer>("round(FreightDouble) eq 32")));
        }

        [TestMethod]
        public void RoundNullable()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimalNullable.Value) == 32), Filter.Deserialize<Customer>("round(FreightDecimalNullable) eq 32")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDoubleNullable.Value) == 32), Filter.Deserialize<Customer>("round(FreightDoubleNullable) eq 32")));
        }

        [TestMethod]
        public void Ceiling()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimal) == 33), Filter.Deserialize<Customer>("ceiling(FreightDecimal) eq 33")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDouble) == 33), Filter.Deserialize<Customer>("ceiling(FreightDouble) eq 33")));
        }

        [TestMethod]
        public void CeilingNullable()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimalNullable.Value) == 33), Filter.Deserialize<Customer>("ceiling(FreightDecimalNullable) eq 33")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDoubleNullable.Value) == 33), Filter.Deserialize<Customer>("ceiling(FreightDoubleNullable) eq 33")));
        }

        [TestMethod]
        public void Floor()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimal) == 34), Filter.Deserialize<Customer>("floor(FreightDecimal) eq 34")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDouble) == 34), Filter.Deserialize<Customer>("floor(FreightDouble) eq 34")));
        }

        [TestMethod]
        public void FloorNullable()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimalNullable.Value) == 34), Filter.Deserialize<Customer>("floor(FreightDecimalNullable) eq 34")));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDoubleNullable.Value) == 34), Filter.Deserialize<Customer>("floor(FreightDoubleNullable) eq 34")));
        }
    }
}
