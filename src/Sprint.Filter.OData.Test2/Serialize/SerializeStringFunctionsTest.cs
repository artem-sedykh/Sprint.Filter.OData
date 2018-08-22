using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeStringFunctionsTest
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
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true)), "substringof('Alfreds', Name) eq true");
        }

        [Test]
        public void EndsWith()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste"))), "endswith(Name, 'Futterkiste')");            
        }

        [Test]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true)), "startswith(Name, 'Alfr') eq true");                        
        }

        [Test]
        public void Length()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Length == 19)), "length(Name) eq 19");            
        }

        [Test]
        public void TestIndexOf()
        {
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1)), "indexof(Name, 'lfreds') eq 1");                      
        }

        [Test]
        public void Replace()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste")), "replace(Name, ' ', '') eq 'AlfredsFutterkiste'");            
        }


        [Test]
        public void SubstringWithOneArgument()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste")), "substring(Name, 1) eq 'lfreds Futterkiste'");            
        }

        [Test]
        public void SubstringWithTwoArgument()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf")), "substring(Name, 1, 2) eq 'lf'");            
        }

        [Test]
        public void ToLower()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste")), "tolower(Name) eq 'alfreds futterkiste'");            
        }

        [Test]
        public void ToUpper()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE")), "toupper(Name) eq 'ALFREDS FUTTERKISTE'");            
        }

        [Test]
        public void Trim()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste")), "trim(Name) eq 'Alfreds Futterkiste'");            
        }

        [Test]
        public void Concat()
        {
            Assert.AreEqual(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany")), "concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'");            
        }
    }
}
