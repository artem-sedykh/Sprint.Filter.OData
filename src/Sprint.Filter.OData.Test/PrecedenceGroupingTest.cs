﻿using Xunit;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test
{
    
    public class PrecedenceGroupingTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        
        public PrecedenceGroupingTest()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void Brackets()
        {
            Assert.True(ExpressionEqualityComparer.Equals(Filter.Deserialize<Customer>("Id mul (3 add Id) ge -15"), Linq.Linq.Expr<Customer, bool>(t => t.Id * (3 + t.Id) >= -15)));
        }
    }
}
