using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    
    public class StringFunctionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public StringFunctionsTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Substringof()
        {            
            // ReSharper disable once RedundantBoolCompare
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Contains("Alfreds") == true), Filter.Deserialize<Customer>("substringof('Alfreds', Name) eq true")));
        }

        [Fact]
        public void EndsWith()
        {            
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.EndsWith("Futterkiste")), Filter.Deserialize<Customer>("endswith(Name, 'Futterkiste')")));
        }

        [Fact]
        public void StartsWith()
        {
            // ReSharper disable once RedundantBoolCompare
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.StartsWith("Alfr") == true), Filter.Deserialize<Customer>("startswith(Name, 'Alfr') eq true")));
        }

        [Fact]
        public void Length()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Length == 19), Filter.Deserialize<Customer>("length(Name) eq 19")));
        }

        [Fact]
        public void TestIndexOf()
        {            
            // ReSharper disable StringIndexOfIsCultureSpecific.1
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.IndexOf("lfreds") == 1), Filter.Deserialize<Customer>("indexof(Name, 'lfreds') eq 1")));
        }

        [Fact]
        public void Replace()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Replace(" ", "") == "AlfredsFutterkiste"), Filter.Deserialize<Customer>("replace(Name, ' ', '') eq 'AlfredsFutterkiste'")));
        }


        [Fact]
        public void SubstringWithOneArgument()
        {            
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1) == "lfreds Futterkiste"), Filter.Deserialize<Customer>("substring(Name, 1) eq 'lfreds Futterkiste'")));
        }

        [Fact]
        public void SubstringWithTwoArgument()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Substring(1, 2) == "lf"), Filter.Deserialize<Customer>("substring(Name, 1, 2) eq 'lf'")));
        }

        [Fact]
        public void ToLower()
        {            
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToLower() == "alfreds futterkiste"), Filter.Deserialize<Customer>("tolower(Name) eq 'alfreds futterkiste'")));
        }

        [Fact]
        public void ToUpper()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.ToUpper() == "ALFREDS FUTTERKISTE"), Filter.Deserialize<Customer>("toupper(Name) eq 'ALFREDS FUTTERKISTE'")));
        }

        [Fact]
        public void Trim()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Name.Trim() == "Alfreds Futterkiste"), Filter.Deserialize<Customer>("trim(Name) eq 'Alfreds Futterkiste'")));
        }

        [Fact]
        public void Concat()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => (t.Name + ", " + t.LastName) == "Berlin, Germany"), Filter.Deserialize<Customer>("concat(concat(Name, ', '), LastName) eq 'Berlin, Germany'")));
        }
    }
}
