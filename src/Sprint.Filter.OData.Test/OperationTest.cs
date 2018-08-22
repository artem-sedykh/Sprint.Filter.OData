using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestClass]
    public class OperationTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        //[TestMethod]
        public void Negation()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => -t.Id > 0), Filter.Deserialize<Customer>("-Id gt 0")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => -t.FreightDoubleNullable > 0), Filter.Deserialize<Customer>("-FreightDoubleNullable gt 0")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => -t.FreightDoubleNullable > 0), Filter.Deserialize<Customer>("- FreightDoubleNullable gt 0")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => -t.FreightDoubleNullable > 0), Filter.Deserialize<Customer>("- FreightDoubleNullable gt 0")));
        }

       // [TestMethod]
        public void Equal()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => String.Compare(t.Name, t.LastName, StringComparison.Ordinal) > 0 ), Filter.Deserialize<Customer>("Name gt LastName")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name != t.LastName), Filter.Deserialize<Customer>("Name ne LastName")));


            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Id == 15), Filter.Deserialize<Customer>("Id eq 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDoubleNullable == t.FreightDouble), Filter.Deserialize<Customer>("FreightDoubleNullable eq FreightDouble")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte == 15), Filter.Deserialize<Customer>("Sbyte eq 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Char == 15), Filter.Deserialize<Customer>("Char eq 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte == 15), Filter.Deserialize<Customer>("NullableSbyte eq 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableChar == 15), Filter.Deserialize<Customer>("NullableChar eq 15")));

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Id == null), Filter.Deserialize<Customer>("Id eq null")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => null > t.Id), Filter.Deserialize<Customer>("null gt Id")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => null == t.FreightDecimalNullable), Filter.Deserialize<Customer>("null eq FreightDecimalNullable")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable == null), Filter.Deserialize<Customer>("FreightDecimalNullable eq null")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name == null), Filter.Deserialize<Customer>("Name eq null")));
            //Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => null == t.Name), Filter.Deserialize<Customer>("null eq Name")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData == null), Filter.Deserialize<Customer>("NullableEnumData eq null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableEnumData == (EnumData.TestData1 | EnumData.TestData2)), Filter.Deserialize<Customer>("NullableEnumData eq Sprint.Filter.OData.Test.Models.EnumData'TestData1,TestData2'")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumData == (EnumData.TestData1 | EnumData.TestData2)), Filter.Deserialize<Customer>("EnumData eq Sprint.Filter.OData.Test.Models.EnumData'TestData1,TestData2'")));                       

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataShort == (EnumDataShort.TestData1 | EnumDataShort.TestData2)), Filter.Deserialize<Customer>("EnumDataShort eq Sprint.Filter.OData.Test.Models.EnumDataShort'TestData1,TestData2'")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.EnumDataLong == (EnumDataLong.TestData1 | EnumDataLong.TestData2)), Filter.Deserialize<Customer>("EnumDataLong eq Sprint.Filter.OData.Test.Models.EnumDataLong'TestData1,TestData2'")));
        }

        [TestMethod]
        public void NotEqual()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Id != 15), Filter.Deserialize<Customer>("Id ne 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDoubleNullable != t.FreightDouble), Filter.Deserialize<Customer>("FreightDoubleNullable ne FreightDouble")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte != 15), Filter.Deserialize<Customer>("Sbyte ne 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Char != 15), Filter.Deserialize<Customer>("Char ne 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte != 15), Filter.Deserialize<Customer>("NullableSbyte ne 15")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableChar != 15), Filter.Deserialize<Customer>("NullableChar ne 15")));
        }

      //  [TestMethod]
        public void Modulo()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Salary % 5 == 0), Filter.Deserialize<Customer>("Salary mod 5 eq 0")));
// ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Salary % 5 == null), Filter.Deserialize<Customer>("Salary mod 5 eq null")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == 0), Filter.Deserialize<Customer>("FreightDecimalNullable mod 5 eq 0")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == t.FreightDecimalNullable), Filter.Deserialize<Customer>("FreightDecimalNullable mod 5 eq FreightDecimalNullable")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDecimalNullable % 5 == null), Filter.Deserialize<Customer>("FreightDecimalNullable mod 5 eq null")));
        }

    //    [TestMethod]
        public void Multiplication()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.Sbyte > 10), Filter.Deserialize<Customer>("Sbyte add Sbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.Sbyte == null), Filter.Deserialize<Customer>("Sbyte add Sbyte eq null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.NullableSbyte > 10), Filter.Deserialize<Customer>("NullableSbyte add NullableSbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.NullableSbyte == null), Filter.Deserialize<Customer>("NullableSbyte add NullableSbyte eq null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.Sbyte > 10), Filter.Deserialize<Customer>("NullableSbyte add Sbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.NullableSbyte > 10), Filter.Deserialize<Customer>("Sbyte add NullableSbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte + t.Sbyte > null), Filter.Deserialize<Customer>("NullableSbyte add Sbyte gt null")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte + t.NullableSbyte > null), Filter.Deserialize<Customer>("Sbyte add NullableSbyte gt null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDouble + t.NullableInt > 10), Filter.Deserialize<Customer>("FreightDouble add NullableInt gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.FreightDouble > 10), Filter.Deserialize<Customer>("NullableInt add FreightDouble gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.FreightDouble > t.NullableInt), Filter.Deserialize<Customer>("NullableInt add FreightDouble gt NullableInt")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.NullableInt > 10), Filter.Deserialize<Customer>("NullableInt add NullableInt gt 10")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.NullableInt > 10.0), Filter.Deserialize<Customer>("NullableInt add NullableInt gt 10.0")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.NullableInt > t.NullableInt), Filter.Deserialize<Customer>("NullableInt add NullableInt gt NullableInt")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt + t.NullableInt != null), Filter.Deserialize<Customer>("NullableInt add NullableInt ne null")));
        }

       // [TestMethod]
        public void Division()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.Sbyte > 10), Filter.Deserialize<Customer>("Sbyte div Sbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.Sbyte == null), Filter.Deserialize<Customer>("Sbyte div Sbyte eq null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.NullableSbyte > 10), Filter.Deserialize<Customer>("NullableSbyte div NullableSbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.NullableSbyte == null), Filter.Deserialize<Customer>("NullableSbyte div NullableSbyte eq null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.Sbyte > 10), Filter.Deserialize<Customer>("NullableSbyte div Sbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.NullableSbyte > 10), Filter.Deserialize<Customer>("Sbyte div NullableSbyte gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableSbyte / t.Sbyte > null), Filter.Deserialize<Customer>("NullableSbyte div Sbyte gt null")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Sbyte / t.NullableSbyte > null), Filter.Deserialize<Customer>("Sbyte div NullableSbyte gt null")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.FreightDouble / t.NullableInt > 10), Filter.Deserialize<Customer>("FreightDouble div NullableInt gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.FreightDouble > 10), Filter.Deserialize<Customer>("NullableInt div FreightDouble gt 10")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.FreightDouble > t.NullableInt), Filter.Deserialize<Customer>("NullableInt div FreightDouble gt NullableInt")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.NullableInt > 10), Filter.Deserialize<Customer>("NullableInt div NullableInt gt 10")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.NullableInt > 10.0), Filter.Deserialize<Customer>("NullableInt div NullableInt gt 10.0")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.NullableInt > t.NullableInt), Filter.Deserialize<Customer>("NullableInt div NullableInt gt NullableInt")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.NullableInt / t.NullableInt != null), Filter.Deserialize<Customer>("NullableInt div NullableInt ne null")));
        }

        [TestMethod]
        public void LogicalNegation()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => !(t.FreightDoubleNullable > t.FreightDouble)), Filter.Deserialize<Customer>("not (FreightDoubleNullable gt FreightDouble)")));
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => !t.Name.EndsWith("milk")), Filter.Deserialize<Customer>("not endswith(Name,'milk')")));
        }
    }
}
