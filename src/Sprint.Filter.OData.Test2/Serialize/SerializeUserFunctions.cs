//using System.Data.Entity.SqlServer;
//using System.Linq;
//using NUnit.Framework;
//using Sprint.Filter.OData.Test.Helpers;
//using Sprint.Filter.OData.Test.Models;

//namespace Sprint.Filter.OData.Test.Serialize
//{
//    [TestFixture]
//    public class SerializeUserFunctions
//    {
//        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

//        [SetUp]
//        public void TestInitialize()
//        {
//            ExpressionEqualityComparer = new ExpressionEqualityComparer();

//            var methods = typeof(SqlFunctions).GetMethods().Where(x => x.Name == "StringConvert").ToArray();

//            MethodProvider.UserFunctions.Clear();

//            MethodProvider.RegisterFunction("StringConvert", methods);
//        }

//        [Test]
//        public void EntityFramworkStringConvert()
//        {
//            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => SqlFunctions.StringConvert(t.Price) == "1")),"StringConvert(Price) eq '1'");

//            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => SqlFunctions.StringConvert(t.Price, 2) == "1")), "StringConvert(Price, 2) eq '1'");

//            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => SqlFunctions.StringConvert(t.Price, 2, null) == "1")), "StringConvert(Price, 2, null) eq '1'");

//            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => SqlFunctions.StringConvert(t.Price, null, null) == "1")), "StringConvert(Price, null, null) eq '1'");

//            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => SqlFunctions.StringConvert(t.FreightDecimalNullable, null, null) == "1")), "StringConvert(FreightDecimalNullable, null, null) eq '1'");
//        }
//    }
//}
