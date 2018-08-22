using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sprint.Filter.Extensions;

namespace Sprint.Filter.OData.Test
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        [Test]
        public void IsIQueryable()
        {
            Assert.IsFalse((new List<int>()).GetType().IsIQueryable());

            Assert.IsTrue((new List<int>().AsQueryable()).GetType().IsIQueryable());

            Assert.IsTrue((typeof(IQueryable<int>)).IsIQueryable());

            Assert.IsTrue((typeof(IQueryable<>)).IsIQueryable());

            Assert.IsTrue((typeof(IQueryable)).IsIQueryable());
        }

        [Test]
        public void IsIEnumerable()
        {
            Assert.IsTrue((new List<int>()).GetType().IsIEnumerable());

            Assert.IsTrue((new List<int>().AsQueryable()).GetType().IsIEnumerable());

            Assert.IsTrue((typeof(IQueryable<int>)).IsIEnumerable());

            Assert.IsTrue((typeof(IQueryable<>)).IsIEnumerable());

            Assert.IsTrue((typeof(IEnumerable<>)).IsIEnumerable());

            Assert.IsTrue((typeof(IEnumerable<int>)).IsIEnumerable());

            Assert.IsTrue((typeof(IEnumerable<object>)).IsIEnumerable());

            Assert.IsTrue((typeof(IEnumerable)).IsIEnumerable());
        }

        [Test]
        public void IsOpenType()
        {
            Assert.IsTrue(typeof(List<>).IsOpenType());

            Assert.IsFalse(typeof(List<int>).IsOpenType());

            Assert.IsFalse(typeof(IQueryable<object>).IsOpenType());

            Assert.IsTrue(typeof(IQueryable<>).IsOpenType());

            Assert.IsTrue(typeof(Func<,>).IsOpenType());
        }
    }
}
