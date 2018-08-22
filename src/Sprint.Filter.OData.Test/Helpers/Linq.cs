using System;
using System.Linq.Expressions;

namespace Sprint.Filter.OData.Test.Helpers
{
    public static class Linq
    {
        public static Expression<Func<T, TResult>> Expr<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            return expr;
        }
    }
}
