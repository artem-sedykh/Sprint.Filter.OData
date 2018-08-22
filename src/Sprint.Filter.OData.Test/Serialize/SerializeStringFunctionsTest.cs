using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    
    public class SerializeStringFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Substringof()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true)), "substringof('Alfreds', Name) eq true");
        }

        [Fact]
        public void EndsWith()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste"))), "endswith(Name, 'Futterkiste')");            
        }

        [Fact]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true)), "startswith(Name, 'Alfr') eq true");                        
        }

        [Fact]
        public void Length()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Length == 19)), "length(Name) eq 19");            
        }

        [Fact]
        public void TestIndexOf()
        {
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1)), "indexof(Name, 'lfreds') eq 1");                      
        }

        [Fact]
        public void Replace()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste")), "replace(Name, ' ', '') eq 'AlfredsFutterkiste'");            
        }


        [Fact]
        public void SubstringWithOneArgument()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste")), "substring(Name, 1) eq 'lfreds Futterkiste'");            
        }

        [Fact]
        public void SubstringWithTwoArgument()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf")), "substring(Name, 1, 2) eq 'lf'");            
        }

        [Fact]
        public void ToLower()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste")), "tolower(Name) eq 'alfreds futterkiste'");            
        }

        [Fact]
        public void ToUpper()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE")), "toupper(Name) eq 'ALFREDS FUTTERKISTE'");            
        }

        [Fact]
        public void Trim()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste")), "trim(Name) eq 'Alfreds Futterkiste'");            
        }

        [Fact]
        public void Concat()
        {
            Assert.Equal(Filter.Serialize(Linq.Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany")), "concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'");            
        }
    }
}
