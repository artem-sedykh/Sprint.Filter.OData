using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.OData.Serialize;
using Sprint.Filter.OData.Test.Helpers;
using Sprint.Filter.OData.Test.Models;

namespace Sprint.Filter.OData.Test.Serialize
{    
    [TestClass]
    public class SerializeTranslatorTest
    {

        Translator Translator { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Translator = new Translator();
        }

        [TestMethod]
        public void Test()
        {
            var expr = Linq.Expr<Customer, bool>(t => t.Id == 15);

            var translator = new Translator();

            var query = translator.Translate(expr);

            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void SimpleExpression()
        {
            var expr = Linq.Expr<Customer, bool>(t => t.Id == 15);

            Assert.AreEqual("Id eq 15",Translator.Translate(expr));
        }
        
        [TestMethod]
        public void CaptureVariable()
        {
            var id = 15;

            Assert.AreEqual("Id eq 15", Translator.Translate(Linq.Expr<Customer, bool>(t => t.Id == 15)));

            Assert.AreEqual(String.Format("{0} eq 15", DateTime.Now.Day), Translator.Translate(Linq.Expr<Customer, bool>(t => DateTime.Now.Day == 15)));

            Assert.AreEqual("Id eq 15", Translator.Translate(Linq.Expr<Customer, bool>(t => t.Id == id)));
             
            var dateTimeValue = DateTime.Now;
            var res = String.Format("datetime'{0}'", XmlConvert.ToString(dateTimeValue, XmlDateTimeSerializationMode.Utc));
            Assert.AreEqual("BirthDate eq " + res, Translator.Translate(Linq.Expr<Customer, bool>(t => t.BirthDate == dateTimeValue)));
        }


        [TestMethod]
        public void Contains()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            var preTranslator = new PreExpressionTranslator();

            
            //var expr = Linq.Expr<Customer, bool>(x => array.AsQueryable().Where(d=>d>4).Contains(x.Id) && x.Id==15 && DateTime.Now.GetDateTimeFormats().Contains(x.Name));

            var expr = Linq.Expr<Customer, bool>(x => array.AsQueryable().Where(d => d > 4).Contains(x.Id) || x.Customers.Contains(x.Parent) || array.Contains(5));
           

           var query = Translator.Translate(expr);
        }

        [TestMethod]
        public void Any()
        {
            var expr = Linq.Expr<Customer, bool>(x =>x.Customers.Any() || x.Items.Any() ||  x.Customers.Any(a => a.Id > 15) || x.Items.Any(c=>c.Id==13) || x.Customers.Any(c=>c.Customers.Any(c2=>c2.Id==x.Id)));
       
            Assert.AreEqual(Translator.Translate(expr), "Customers/Any() or Items/Any() or Customers/Any(a: a/Id gt 15) or Items/Any(c: c/Id eq 13) or Customers/Any(c: c/Customers/Any(c2: c2/Id eq Id))");
        }


        [TestMethod]
        public void All()
        {
            var expr = Linq.Expr<Customer, bool>(x => x.Customers.All(a => a.Id > 15) || x.Items.All(c => c.Id == 13) || x.Customers.All(c => c.Customers.All(c2 => c2.Id == x.Id)));

            Assert.AreEqual(Translator.Translate(expr), "Customers/All(a: a/Id gt 15) or Items/All(c: c/Id eq 13) or Customers/All(c: c/Customers/All(c2: c2/Id eq Id))");
        }        



    }
}
