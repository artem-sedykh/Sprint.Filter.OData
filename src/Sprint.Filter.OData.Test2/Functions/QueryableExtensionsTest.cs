using System.Linq;
using NUnit.Framework;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Functions
{
    [TestFixture]
    public class QueryableExtensionsTest
    {
        public ExpressionEqualityComparer ExpressionEqualityComparer { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            ExpressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Test]
        public void First()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.First().Id == 15),
                Filter.Deserialize<Customer>("Customers/First()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.First(c=>c.Name=="test").Id == 15),
                Filter.Deserialize<Customer>("Customers/First(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void FirstOrDefault()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.FirstOrDefault().Id == 15),
                Filter.Deserialize<Customer>("Customers/FirstOrDefault()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.FirstOrDefault(c => c.Name == "test").Id == 15),
                Filter.Deserialize<Customer>("Customers/FirstOrDefault(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void Last()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Last().Id == 15),
                Filter.Deserialize<Customer>("Customers/Last()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Last(c => c.Name == "test").Id == 15),
                Filter.Deserialize<Customer>("Customers/Last(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void LastOrDefault()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.LastOrDefault().Id == 15),
                Filter.Deserialize<Customer>("Customers/LastOrDefault()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.LastOrDefault(c => c.Name == "test").Id == 15),
                Filter.Deserialize<Customer>("Customers/LastOrDefault(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void Single()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Single().Id == 15),
                Filter.Deserialize<Customer>("Customers/Single()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Single(c => c.Name == "test").Id == 15),
                Filter.Deserialize<Customer>("Customers/Single(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void SingleOrDefault()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.SingleOrDefault().Id == 15),
                Filter.Deserialize<Customer>("Customers/SingleOrDefault()/Id eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.SingleOrDefault(c => c.Name == "test").Id == 15),
                Filter.Deserialize<Customer>("Customers/SingleOrDefault(c: c/Name eq 'test')/Id eq 15")));
        }

        [Test]
        public void Count()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.IntArray.Count() == 15),
              Filter.Deserialize<Customer>("IntArray/Count() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Count() == 15),
                Filter.Deserialize<Customer>("Customers/Count() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Count(c => c.Name == "test") == 15),
                Filter.Deserialize<Customer>("Customers/Count(c: c/Name eq 'test') eq 15")));
        }

        [Test]
        public void LongCount()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.IntArray.LongCount() == 15),
               Filter.Deserialize<Customer>("IntArray/LongCount() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.LongCount() == 15),
                Filter.Deserialize<Customer>("Customers/LongCount() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.LongCount(c => c.Name == "test") == 15),
                Filter.Deserialize<Customer>("Customers/LongCount(c: c/Name eq 'test') eq 15")));
        }

        [Test]
        public void Min()
        {                   
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Min() == 15),
               Filter.Deserialize<Customer>("Numbers/Min() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Min(x => x.Id) == 15),
              Filter.Deserialize<Customer>("Customers/Min(x: x/Id) eq 15")));
        }

        [Test]
        public void Max()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Max() == 15),
               Filter.Deserialize<Customer>("Numbers/Max() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Max(x => x) == 15),
               Filter.Deserialize<Customer>("Numbers/Max(x: x) eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Max(x => x.Id) == 15),
              Filter.Deserialize<Customer>("Customers/Max(x: x/Id) eq 15")));
        }

        [Test]
        public void Sum()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Sum(x => x.Id) == 15),
             Filter.Deserialize<Customer>("Customers/Sum(x: x/Id) eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Sum() == 15),
               Filter.Deserialize<Customer>("Numbers/Sum() eq 15")));            
        }

        [Test]
        public void Average()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals( 
                
                Linq.Linq.Expr<Customer, bool>(t => t.IntArray.Average(x => x) == 15d), 

                Filter.Deserialize<Customer>("IntArray/Average(x: x) eq 15")));

// ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Average(x => x.Id) == 15d),
             Filter.Deserialize<Customer>("Customers/Average(x: x/Id) eq 15")));

// ReSharper disable once CompareOfFloatsByEqualityOperator
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Average() == 15d),
               Filter.Deserialize<Customer>("Numbers/Average() eq 15")));
        }

        [Test]
        public void GroupBy()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.GroupBy(x => x.Id).Count() == 15), 
                Filter.Deserialize<Customer>("Customers/GroupBy(x: x/Id)/Count() eq 15")));
        }

        [Test]
        public void SelectMany()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.SelectMany(x => x.Items).Count() == 15),
                Filter.Deserialize<Customer>("Customers/SelectMany(x: x/Items)/Count() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Items.SelectMany(x => x.Customers).Count() == 15),
                Filter.Deserialize<Customer>("Items/SelectMany(x: x/Customers)/Count() eq 15")));
        }

        [Test]
        public void Select()
        {            
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.IntArray.Select(x => x).Count() == 15),
                Filter.Deserialize<Customer>("IntArray/Select(x: x)/Count() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => x.Id).Count() == 15),
                Filter.Deserialize<Customer>("Customers/Select(x: x/Id)/Count() eq 15")));
        }

        [Test]
        public void Distinct()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Select(x => x.Id).Distinct().Count() == 15),
                Filter.Deserialize<Customer>("Customers/Select(x: x/Id)/Distinct()/Count() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Distinct().Count() == 15),
                Filter.Deserialize<Customer>("Numbers/Distinct()/Count() eq 15")));
        }

        [Test]
        public void Where()
        {
            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.IntArray.Where(x => x > 2).Count() == 15),
                Filter.Deserialize<Customer>("IntArray/Where(x: x gt 2)/Count() eq 15")));

            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Where(x => x.Id > 2).Count() == 15),
                Filter.Deserialize<Customer>("Customers/Where(x: x/Id gt 2)/Count() eq 15")));

            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Where(x=> x >10).Count() == 15),
                Filter.Deserialize<Customer>("Numbers/Where(x: x gt 10)/Count() eq 15")));
        }

        [Test]
        public void Take()
        {
            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Take(5).Count() == 5),
                Filter.Deserialize<Customer>("Customers/Take(5)/Count() eq 5")));

            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Take(5).Count() == 5),
                Filter.Deserialize<Customer>("Numbers/Take(5)/Count() eq 5")));
        }

        [Test]
        public void Skip()
        {
            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Skip(5).Count() == 5),
                Filter.Deserialize<Customer>("Customers/Skip(5)/Count() eq 5")));

            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Skip(5).Count() == 5),
                Filter.Deserialize<Customer>("Numbers/Skip(5)/Count() eq 5")));
        }

        [Test]
        public void DefaultIfEmpty()
        {
            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.DefaultIfEmpty().Count() == 5),
                Filter.Deserialize<Customer>("Customers/DefaultIfEmpty()/Count() eq 5")));

            // ReSharper disable once ReplaceWithSingleCallToCount
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.DefaultIfEmpty().Count() == 5),
                Filter.Deserialize<Customer>("Numbers/DefaultIfEmpty()/Count() eq 5")));
        }

        [Test]
        public void OrderBy()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.OrderBy(x => x.Id).Select(x=>x.Id).First() == 15),
               Filter.Deserialize<Customer>("Customers/OrderBy(x: x/Id)/Select(x: x/Id)/First() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Distinct().OrderBy(x=>x).First() == 15),
                Filter.Deserialize<Customer>("Numbers/Distinct()/OrderBy(x: x)/First() eq 15")));
        }

        [Test]
        public void ThenBy()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.OrderBy(x => x.Id).ThenBy(x=>x.Name).Select(x => x.Id).First() == 15),
               Filter.Deserialize<Customer>("Customers/OrderBy(x: x/Id)/ThenBy(x: x/Name)/Select(x: x/Id)/First() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Distinct().OrderBy(x => x).ThenBy(x => x).First() == 15),
                Filter.Deserialize<Customer>("Numbers/Distinct()/OrderBy(x: x)/ThenBy(x: x)/First() eq 15")));
        }

        [Test]
        public void OrderByDescending()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.OrderByDescending(x => x.Id).Select(x => x.Id).First() == 15),
               Filter.Deserialize<Customer>("Customers/OrderByDescending(x: x/Id)/Select(x: x/Id)/First() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Distinct().OrderByDescending(x => x).First() == 15),
                Filter.Deserialize<Customer>("Numbers/Distinct()/OrderByDescending(x: x)/First() eq 15")));
        }

        [Test]
        public void ThenByDescending()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.OrderBy(x => x.Id).ThenByDescending(x => x.Name).Select(x => x.Id).First() == 15),
               Filter.Deserialize<Customer>("Customers/OrderBy(x: x/Id)/ThenByDescending(x: x/Name)/Select(x: x/Id)/First() eq 15")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Numbers.Distinct().OrderBy(x => x).ThenByDescending(x => x).First() == 15),
                Filter.Deserialize<Customer>("Numbers/Distinct()/OrderBy(x: x)/ThenByDescending(x: x)/First() eq 15")));
        }

        [Test]
        public void Contains()
        {
            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Contains(t.Parent)),
               Filter.Deserialize<Customer>("Customers/Contains(Parent)")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Contains(t)),
               Filter.Deserialize<Customer>("Customers/Contains()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.Customers.Contains(t)),
                Filter.Deserialize<Customer>("Customers/Contains()")));

            Assert.IsTrue(ExpressionEqualityComparer.Equals(Linq.Linq.Expr<Customer, bool>(t => t.IntArray.Contains(t.Id)),
                Filter.Deserialize<Customer>("IntArray/Contains(Id)")));
        }
    }
}
