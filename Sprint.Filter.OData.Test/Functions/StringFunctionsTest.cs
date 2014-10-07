using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestClass]
    public class StringFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [TestMethod]
        public void Substringof()
        {            
            // ReSharper disable once RedundantBoolCompare
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true), Filter.Deserialize<Customer>("substringof('Alfreds', Name) eq true")));
        }

        [TestMethod]
        public void EndsWith()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste")), Filter.Deserialize<Customer>("endswith(Name, 'Futterkiste')")));
        }

        [TestMethod]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true), Filter.Deserialize<Customer>("startswith(Name, 'Alfr') eq true")));
        }

        [TestMethod]
        public void Length()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Length == 19), Filter.Deserialize<Customer>("length(Name) eq 19")));
        }

        [TestMethod]
        public void TestIndexOf()
        {            
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1), Filter.Deserialize<Customer>("indexof(Name, 'lfreds') eq 1")));
        }

        [TestMethod]
        public void Replace()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste"), Filter.Deserialize<Customer>("replace(Name, ' ', '') eq 'AlfredsFutterkiste'")));
        }


        [TestMethod]
        public void SubstringWithOneArgument()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste"), Filter.Deserialize<Customer>("substring(Name, 1) eq 'lfreds Futterkiste'")));
        }

        [TestMethod]
        public void SubstringWithTwoArgument()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf"), Filter.Deserialize<Customer>("substring(Name, 1, 2) eq 'lf'")));
        }

        [TestMethod]
        public void ToLower()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste"), Filter.Deserialize<Customer>("tolower(Name) eq 'alfreds futterkiste'")));
        }

        [TestMethod]
        public void ToUpper()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE"), Filter.Deserialize<Customer>("toupper(Name) eq 'ALFREDS FUTTERKISTE'")));
        }

        [TestMethod]
        public void Trim()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste"), Filter.Deserialize<Customer>("trim(Name) eq 'Alfreds Futterkiste'")));
        }

        [TestMethod]
        public void Concat()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany"), Filter.Deserialize<Customer>("concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'")));
        }
    }
}
