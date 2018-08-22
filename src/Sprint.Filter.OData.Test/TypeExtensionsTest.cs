using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Sprint.Filter.Extensions;
using Xunit;

namespace Sprint.Filter.OData.Test
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void IsIQueryable()
        {
            Assert.False((new List<int>()).GetType().IsIQueryable());

            Assert.True((new List<int>().AsQueryable()).GetType().IsIQueryable());

            Assert.True((typeof(IQueryable<int>)).IsIQueryable());

            Assert.True((typeof(IQueryable<>)).IsIQueryable());

            Assert.True((typeof(IQueryable)).IsIQueryable());
        }

        [Fact]
        public void IsIEnumerable()
        {
            Assert.True((new List<int>()).GetType().IsIEnumerable());

            Assert.True((new List<int>().AsQueryable()).GetType().IsIEnumerable());

            Assert.True((typeof(IQueryable<int>)).IsIEnumerable());

            Assert.True((typeof(IQueryable<>)).IsIEnumerable());

            Assert.True((typeof(IEnumerable<>)).IsIEnumerable());

            Assert.True((typeof(IEnumerable<int>)).IsIEnumerable());

            Assert.True((typeof(IEnumerable<object>)).IsIEnumerable());

            Assert.True((typeof(IEnumerable)).IsIEnumerable());
        }

        [Fact]
        public void IsOpenType()
        {
            Assert.True(typeof(List<>).IsOpenType());

            Assert.False(typeof(List<int>).IsOpenType());

            Assert.False(typeof(IQueryable<object>).IsOpenType());

            Assert.True(typeof(IQueryable<>).IsOpenType());

            Assert.True(typeof(Func<,>).IsOpenType());
        }
    }
}
