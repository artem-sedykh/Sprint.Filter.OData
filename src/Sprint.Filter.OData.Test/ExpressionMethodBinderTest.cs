using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprint.Filter.Helpers;

namespace Sprint.Filter.OData.Test
{
    
    [TestClass]
    public class ExpressionMethodBinderTest
    {
        #region Test Data

        class PetOwner
        {
            public string Name { get; set; }
            public List<string> Pets { get; set; }
        }

        public class B<T> : A<T> {}

        public class A<T>{}

        public class Source
        {
            public static void Test1<TModel, TResult>(Func<A<object>, TResult> model, TModel data) { }

            public static void Test2<TModel, TResult>(Expression<Func<IEnumerable<object>, TResult>> model, TModel data) { }
        }

        #endregion

        [TestMethod]
        public void Bind()
        {
            Expression<Func<IEnumerable<object>, int>> e1 = x => 0;
            
            var method = ExpressionMethodBinder.Bind(typeof(Source), "Test2",null, new Expression[] { Expression.Constant(e1), Expression.Constant(1) });

            Assert.IsNotNull(method);

        }

        public void SelectManyBind()
        {
            PetOwner[] petOwners =
                    { new PetOwner { Name="Higa", 
                          Pets = new List<string>{ "Scruffy", "Sam" } },
                      new PetOwner { Name="Ashkenazi", 
                          Pets = new List<string>{ "Walker", "Sugar" } },
                      new PetOwner { Name="Price", 
                          Pets = new List<string>{ "Scratches", "Diesel" } },
                      new PetOwner { Name="Hines", 
                          Pets = new List<string>{ "Dusty" } } };

            Expression<Func<PetOwner, List<string>>> e2 = petOwner => petOwner.Pets;
            Expression<Func<PetOwner, string, string>> e3 = (petOwner, petName) => petName + petOwner;

            var method = ExpressionMethodBinder.Bind(typeof(Enumerable), "SelectMany", null, new Expression[]
            {
                Expression.Constant(petOwners), e2, e3

            });

            Assert.IsNotNull(method);
        }
    }
}
