using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Test.QueryProviderTests
{

    public class ODataQuery<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable
    {
        private readonly QueryProvider _queryProvider;
        private readonly Expression _expression;
        private readonly Uri _adress;

        internal ODataQuery(Expression expression, QueryProvider provider)
        {
            _expression = expression;

            _queryProvider = provider;
        }

        public ODataQuery(Uri adress)
        {
            _queryProvider = new CustomQueryProvider(adress);

            _expression = Expression.Constant(this);

            _adress = adress;
        }

        public Expression Expression { get { return _expression; } }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _queryProvider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new Exception();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return _queryProvider.GetQueryText(null);                     
        }
    }    
}
