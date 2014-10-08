using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{

    [TestClass]
    public class FilterTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }
             
        [TestMethod]
        public void ComplexFunction()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => !t.Customers.Any() || t.Customers.Any(tag =>tag.Id - 1 == t.Id || tag.Customers.Any(c => c.Id == tag.Id) && tag.Customers.Any(c => c.Id == t.Id))),  Filter.Deserialize<Customer>("not Customers/any() or Customers/any(tag: tag/Id sub 1 eq Id or tag/Customers/any(c: c/Id eq tag/Id) and tag/Customers/any(c: c/Id eq Id))")));
                        
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Customers.Any(tag => tag.Id == t.Id || tag.Customers.Any(c => c.Id == tag.Id) || tag.Customers.Any(c => c.Id == t.Id)) || t.Name.Substring(1) == "lfreds Futterkiste"), Filter.Deserialize<Customer>("Customers/any(tag: tag/Id eq Id or tag/Customers/any(c: c/Id eq tag/Id) or tag/Customers/any(c: c/Id eq Id)) or substring(Name, 1) eq 'lfreds Futterkiste'")));
        }

        [TestMethod]
        public void ToStringTest()
        {
// ReSharper disable once SpecifyACultureInStringConversionExplicitly
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Id.ToString() == "15"),
                Filter.Deserialize<Customer>("Id/ToString() eq '15'")));
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Id.Equals(15)),
                Filter.Deserialize<Customer>("Id/Equals(15)")));
        }

        [TestMethod]
        public void GroupBy()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Customers.GroupBy(x=>x.Name).Count()==1),
                Filter.Deserialize<Customer>("Customers/groupby(x: x/Name)/Count() eq 1")));
        }

        [TestMethod]
        public void CustomName()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.CustomName1 == 1 || t.CustomName2==2 || t.CustomName3==3),
                Filter.Deserialize<Customer>("cn1 eq 1 or cn2 eq 2 or cn3 eq 3")));
        }


        [TestMethod]
        public void BooleanOperatorNullableTypes()
        {
            var expr = Filter.Deserialize<Customer>("UnitPrice eq 5.00m or CategoryID eq 0");
        }
    }
}
