using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    [TestFixture]
    public class ConstantTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }
        
        [Test]
        public void DateTime()
        {
            var expr1 = Filter.Invoke<DateTime>("datetime'2000-12-12T12:00'");

            Assert.IsTrue(expr1.Compile()() == new DateTime(2000, 12, 12, 12, 0, 0));           
        }

        [Test]
        public void TimeSpan()
        {
            var value = Filter.Invoke<TimeSpan>("time'P1Y3M5DT7H10M3.3S'").Compile()();

            Assert.AreEqual(XmlConvert.ToTimeSpan("P1Y3M5DT7H10M3.3S"), value);
        }

        [Test]
        public void DateTimeOffset()
        {
            var offset = new DateTimeOffset(new DateTime(2012, 2, 20, 12, 40, 45,327), new TimeSpan(1, 0, 0));
            
            var expr = Filter.Invoke<DateTimeOffset>("datetimeoffset'2012-02-20T12:40:45.327+01:00'");

            var value = expr.Compile()();

            Assert.AreEqual(value, offset);

            Assert.AreEqual(new DateTimeOffset(new DateTime(2012, 2, 20, 12, 40, 45, 327), new TimeSpan(-1, 0, 0)), Filter.Invoke<DateTimeOffset>("datetimeoffset'2012-02-20T12:40:45.327-01:00'").Compile()());
        }

        [Test]
        public void Int16()
        {
            //TODO: подумать нужна ли этп фигня
            //var v = 16;

            //var expr1 = Filter.Invoke<short>("16");

            //Assert.IsTrue(expr1.Compile()() == v);
        }

        [Test]
        public void Int32()
        {
            var expr1 = Filter.Invoke<int>("12");

            Assert.IsTrue(expr1.Compile()() == 12);
        }

        [Test]
        public void Int64()
        {
            var expr1 = Filter.Invoke<long>("12l");

            Assert.IsTrue(expr1.Compile()() == 12l);

            var expr2 = Filter.Invoke<long>("12L");

            Assert.IsTrue(expr2.Compile()() == 12L);

            Assert.AreEqual(Filter.Invoke<long>("-12L").Compile()(), -12L);
        }

        [Test]
        public void Double()
        {
            var expr1 = Filter.Invoke<double>("12.0");

            var expr2 = Filter.Invoke<double>("12d");

            Assert.IsTrue(expr2.Compile()() == 12d);

            Assert.IsTrue(expr1.Compile()() == 12.0);
        }

        [Test]
        public void Decimal()
        {
            var expr1 = Filter.Invoke<decimal>("12.1m");

            var expr2 = Filter.Invoke<decimal>("12m");

            Assert.IsTrue(expr2.Compile()() == 12m);

            Assert.IsTrue(expr1.Compile()() == 12.1m);

            Assert.AreEqual(Filter.Invoke<decimal>("-12.1m").Compile()(), -12.1m);
        }

        [Test]
        public void Single()
        {
            var expr1 = Filter.Invoke<float>("2.0f");

            Assert.IsTrue(expr1.Compile()() == 2f);
        }

        [Test]
        public void String()
        {
            var expr1 = Filter.Invoke<string>("'test string'");

            Assert.IsTrue(expr1.Compile()() == "test string");
        }

        [Test]
        public void Binary()
        {            
            var expr1 = Filter.Invoke<byte[]>("X'23AB'");

            CollectionAssert.AreEqual(expr1.Compile()(), new byte[] { 35, 171 });

            var expr2 = Filter.Invoke<byte[]>("binary'23ABFF'");

            CollectionAssert.AreEqual(expr2.Compile()(), new byte[]{35,171, 255});
        }

        [Test]
        public void Boolean()
        {
            var expr1 = Filter.Invoke<bool>("true");

            var expr2 = Filter.Invoke<bool>("false");


            Assert.IsTrue(expr1.Compile()());

            Assert.IsFalse(expr2.Compile()());            
        }

        [Test]
        public void Null()
        {
            var expr1 = Filter.Invoke<object>("null");

            Assert.IsNull(expr1.Compile()());
        }

        [Test]
        public void Guid()
        {
            var guid = System.Guid.NewGuid();

            var expr1 = Filter.Invoke<Guid>(System.String.Format("guid'{0}'", guid));

            Assert.IsTrue(guid==expr1.Compile()());            
        }

        [Test]
        public void Enum()
        {            
            Assert.AreEqual(EnumData.TestData1, Filter.Invoke<EnumData>("Sprint.Filter.OData.Test.Models.EnumData'TestData1'").Compile()());

            Assert.AreEqual(EnumData.TestData1 | EnumData.TestData2, Filter.Invoke<EnumData>("Sprint.Filter.OData.Test.Models.EnumData'TestData1,TestData2'").Compile()());
        }
    }
}
