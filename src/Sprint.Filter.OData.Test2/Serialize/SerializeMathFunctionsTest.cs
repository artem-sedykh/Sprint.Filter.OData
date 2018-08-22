using System;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeMathFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [Test]
        public void Round()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimal) == 32)), "round(FreightDecimal) eq 32m");

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDouble) == 32)), "round(FreightDouble) eq 32");
        }

        [Test]
        public void RoundNullable()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDecimalNullable.Value) == 32)), "round(FreightDecimalNullable/Value) eq 32m");

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Round(t.FreightDoubleNullable.Value) == 32)), "round(FreightDoubleNullable/Value) eq 32");           
        }

        [Test]
        public void Ceiling()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimal) == 32)), "ceiling(FreightDecimal) eq 32m");

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDouble) == 32)), "ceiling(FreightDouble) eq 32");            
        }

        [Test]
        public void CeilingNullable()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDecimalNullable.Value) == 32)), "ceiling(FreightDecimalNullable/Value) eq 32m");

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Ceiling(t.FreightDoubleNullable.Value) == 32)), "ceiling(FreightDoubleNullable/Value) eq 32");              
        }

        [Test]
        public void Floor()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimal) == 34)), "floor(FreightDecimal) eq 34m");            

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDouble) == 34)), "floor(FreightDouble) eq 34");
        }

        [Test]
        public void FloorNullable()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDecimalNullable.Value) == 34)), "floor(FreightDecimalNullable/Value) eq 34m");

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => Math.Floor(t.FreightDoubleNullable.Value) == 34)), "floor(FreightDoubleNullable/Value) eq 34");
        }
    }
}
