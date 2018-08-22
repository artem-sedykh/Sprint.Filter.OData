using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestFixture]
    public class StringFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void Substringof()
        {            
            // ReSharper disable once RedundantBoolCompare
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true), Filter.Deserialize<Customer>("substringof('Alfreds', Name) eq true")));
        }

        [Test]
        public void EndsWith()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste")), Filter.Deserialize<Customer>("endswith(Name, 'Futterkiste')")));
        }

        [Test]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true), Filter.Deserialize<Customer>("startswith(Name, 'Alfr') eq true")));
        }

        [Test]
        public void Length()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Length == 19), Filter.Deserialize<Customer>("length(Name) eq 19")));
        }

        [Test]
        public void TestIndexOf()
        {            
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1), Filter.Deserialize<Customer>("indexof(Name, 'lfreds') eq 1")));
        }

        [Test]
        public void Replace()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste"), Filter.Deserialize<Customer>("replace(Name, ' ', '') eq 'AlfredsFutterkiste'")));
        }


        [Test]
        public void SubstringWithOneArgument()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste"), Filter.Deserialize<Customer>("substring(Name, 1) eq 'lfreds Futterkiste'")));
        }

        [Test]
        public void SubstringWithTwoArgument()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf"), Filter.Deserialize<Customer>("substring(Name, 1, 2) eq 'lf'")));
        }

        [Test]
        public void ToLower()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste"), Filter.Deserialize<Customer>("tolower(Name) eq 'alfreds futterkiste'")));
        }

        [Test]
        public void ToUpper()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE"), Filter.Deserialize<Customer>("toupper(Name) eq 'ALFREDS FUTTERKISTE'")));
        }

        [Test]
        public void Trim()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste"), Filter.Deserialize<Customer>("trim(Name) eq 'Alfreds Futterkiste'")));
        }

        [Test]
        public void Concat()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany"), Filter.Deserialize<Customer>("concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'")));
        }
    }
}
