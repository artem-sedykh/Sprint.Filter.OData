using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeOperationTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void Negation()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => -t.Id > 0)), "-Id gt 0");

            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => -t.FreightDoubleNullable > 0)), "-FreightDoubleNullable gt 0");            
        }

        [Test]
        public void LogicalNegation()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => !(t.FreightDoubleNullable > t.FreightDouble))), "not (FreightDoubleNullable gt FreightDouble)");

            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => !t.Name.EndsWith("milk"))), "not endswith(Name, 'milk')");
        }

        [Test]
        public void Equal()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData == (EnumData.TestData1 | EnumData.TestData2))), "NullableEnumData eq 6");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataShort == (EnumDataShort.TestData1 | EnumDataShort.TestData2))), "EnumDataShort eq 6");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataLong == (EnumDataLong.TestData1 | EnumDataLong.TestData2))), "EnumDataLong eq 6");

            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => null == t.FreightDecimalNullable)), "null eq FreightDecimalNullable");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name != t.LastName)), "Name ne LastName");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Id == 15)), "Id eq 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDoubleNullable == t.FreightDouble)), "FreightDoubleNullable eq FreightDouble");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte == 15)), "Sbyte eq 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Char == 15)), "Char eq 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte == 15)), "NullableSbyte eq 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableChar == 15)), "NullableChar eq 15");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Id == null)), "Id eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => null > t.Id)), "null gt Id");            
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable == null)), "FreightDecimalNullable eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name == null)), "Name eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => null == t.Name)), "null eq Name");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData == null)), "NullableEnumData eq null");            
        }

        [Test]
        public void EquNotEqualal()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => null != t.FreightDecimalNullable)), "null ne FreightDecimalNullable");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name != t.LastName)), "Name ne LastName");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Id != 15)), "Id ne 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDoubleNullable != t.FreightDouble)), "FreightDoubleNullable ne FreightDouble");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte != 15)), "Sbyte ne 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Char != 15)), "Char ne 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte != 15)), "NullableSbyte ne 15");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableChar != 15)), "NullableChar ne 15");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Id != null)), "Id ne null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable != null)), "FreightDecimalNullable ne null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name != null)), "Name ne null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => null != t.Name)), "null ne Name");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData != null)), "NullableEnumData ne null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData != (EnumData.TestData1 | EnumData.TestData2))), "NullableEnumData ne 6");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataShort != (EnumDataShort.TestData1 | EnumDataShort.TestData2))), "EnumDataShort ne 6");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataLong != (EnumDataLong.TestData1 | EnumDataLong.TestData2))), "EnumDataLong ne 6");
        }

        [Test]
        public void Modulo()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Salary % 5 == 0)), "Salary mod 5m eq 0m");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Salary % 5 == null)), "Salary mod 5m eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == 0)), "FreightDecimalNullable mod 5m eq 0m");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == t.FreightDecimalNullable)), "FreightDecimalNullable mod 5m eq FreightDecimalNullable");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == null)), "FreightDecimalNullable mod 5m eq null");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.FreightDouble % 5 == null)), "FreightDouble mod 5 eq null");            
        }

        [Test]
        public void Multiplication()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.NullableSbyte > 10)), "NullableSbyte add NullableSbyte gt 10");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.Sbyte > 10)), "Sbyte add Sbyte gt 10");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.Sbyte == null)), "Sbyte add Sbyte eq null");            
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.NullableSbyte == null)), "NullableSbyte add NullableSbyte eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.Sbyte > 10)), "NullableSbyte add Sbyte gt 10");       
        }

        [Test]
        public void Division()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.NullableSbyte > 10)), "NullableSbyte div NullableSbyte gt 10");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.Sbyte > 10)), "Sbyte div Sbyte gt 10");
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.Sbyte == null)), "Sbyte div Sbyte eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.NullableSbyte == null)), "NullableSbyte div NullableSbyte eq null");
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.Sbyte > 10)), "NullableSbyte div Sbyte gt 10");
        }
    }
}
