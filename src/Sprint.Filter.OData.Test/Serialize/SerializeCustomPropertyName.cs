﻿using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{
    
    public class SerializeCustomPropertyName
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void CustomName()
        {
            var query = Filter.Serialize<Customer>(t => t.CustomName1 == 1 || t.CustomName2 == 2 || t.CustomName3 == 3);

            Assert.Equal(query, "(cn1 eq 1 or cn2 eq 2) or cn3 eq 3");
        }
    }
}
