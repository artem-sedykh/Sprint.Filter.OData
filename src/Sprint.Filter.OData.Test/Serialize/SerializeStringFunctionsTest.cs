using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestClass]
    public class SerializeStringFunctionsTest
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
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true)), "substringof('Alfreds', Name) eq true");
        }

        [TestMethod]
        public void EndsWith()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste"))), "endswith(Name, 'Futterkiste')");            
        }

        [TestMethod]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true)), "startswith(Name, 'Alfr') eq true");                        
        }

        [TestMethod]
        public void Length()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Length == 19)), "length(Name) eq 19");            
        }

        [TestMethod]
        public void TestIndexOf()
        {
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1)), "indexof(Name, 'lfreds') eq 1");                      
        }

        [TestMethod]
        public void Replace()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste")), "replace(Name, ' ', '') eq 'AlfredsFutterkiste'");            
        }


        [TestMethod]
        public void SubstringWithOneArgument()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste")), "substring(Name, 1) eq 'lfreds Futterkiste'");            
        }

        [TestMethod]
        public void SubstringWithTwoArgument()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf")), "substring(Name, 1, 2) eq 'lf'");            
        }

        [TestMethod]
        public void ToLower()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste")), "tolower(Name) eq 'alfreds futterkiste'");            
        }

        [TestMethod]
        public void ToUpper()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE")), "toupper(Name) eq 'ALFREDS FUTTERKISTE'");            
        }

        [TestMethod]
        public void Trim()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste")), "trim(Name) eq 'Alfreds Futterkiste'");            
        }

        [TestMethod]
        public void Concat()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany")), "concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'");            
        }
    }
}
