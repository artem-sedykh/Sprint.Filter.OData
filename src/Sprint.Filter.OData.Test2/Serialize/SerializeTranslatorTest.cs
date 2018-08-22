﻿using System;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Sprint.Filter.OData.Serialize;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;
using Sprint.Linq;

namespace Sprint.Filter.OData.Test.Serialize
{
    [TestFixture]
    public class SerializeTranslatorTest
    {
        QueryTranslator QueryTranslator { get; set; }

        [SetUp]
        public void TestInitialize()
        {
            QueryTranslator = new QueryTranslator();
        }

        [Test]
        public void Test()
        {
            var expr = Linq.Linq.Expr<Customer, bool>(t => t.Id == 15);

            var translator = new QueryTranslator();

            var query = translator.Translate(expr);

            Assert.IsNotNull(query);
        }

        [Test]
        public void SimpleExpression()
        {
            var expr = Linq.Linq.Expr<Customer, bool>(t => t.Id == 15);

            Assert.AreEqual("Id eq 15",QueryTranslator.Translate(expr));
        }
        
        [Test]
        public void CaptureVariable()
        {
            Assert.AreEqual(String.Format("false", DateTime.Now.Day), QueryTranslator.Translate(Linq.Linq.Expr<Customer, bool>(t => DateTime.Now.Day == 15)));

            var id = 15;

            Assert.AreEqual("Id eq 15", QueryTranslator.Translate(Linq.Linq.Expr<Customer, bool>(t => t.Id == 15)));

            Assert.AreEqual("Id eq 15", QueryTranslator.Translate(Linq.Linq.Expr<Customer, bool>(t => t.Id == id)));
             
            var dateTimeValue = DateTime.Now;
            var res = String.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
            Assert.AreEqual("BirthDate eq " + res, QueryTranslator.Translate(Linq.Linq.Expr<Customer, bool>(t => t.BirthDate == dateTimeValue)));
        }


        [Test]
        public void Contains()
        {
            var array = new[] { 1, 2, 3, 4, 5 };            

            
            //var expr = Linq.Linq.Expr<Customer, bool>(x => array.AsQueryable().Where(d=>d>4).Contains(x.Id) && x.Id==15 && DateTime.Now.GetDateTimeFormats().Contains(x.Name));

            var expr = Linq.Linq.Expr<Customer, bool>(x => array.Contains(x.Id) || x.Customers.Contains(x.Parent) || array.Contains(5));
           

           var query = QueryTranslator.Translate(expr);
        }

        [Test]
        public void Any()
        {
            var expr = Linq.Linq.Expr<Customer, bool>(x =>x.Customers.Any() || x.Items.Any() ||  x.Customers.Any(a => a.Id > 15) || x.Items.Any(c=>c.Id==13) || x.Customers.Any(c=>c.Customers.Any(c2=>c2.Id==x.Id)));

            var result = QueryTranslator.Translate(expr);

            Assert.AreEqual(result, "(((Customers/Any() or Items/Any()) or Customers/Any(a: a/Id gt 15)) or Items/Any(c: c/Id eq 13)) or Customers/Any(c: c/Customers/Any(c2: c2/Id eq Id))");
        }


        [Test]
        public void All()
        {
            var expr = Linq.Linq.Expr<Customer, bool>(x => x.Customers.All(a => a.Id > 15) || x.Items.All(c => c.Id == 13) || x.Customers.All(c => c.Customers.All(c2 => c2.Id == x.Id)));

            var result = QueryTranslator.Translate(expr);

            Assert.AreEqual(result, "(Customers/All(a: a/Id gt 15) or Items/All(c: c/Id eq 13)) or Customers/All(c: c/Customers/All(c2: c2/Id eq Id))");
        }
    }
}