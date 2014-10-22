using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Filter.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TModel> ODataQuery<TModel>(this IQueryable<TModel> query, string oDataQuery)
        {
            var filter = "asdas";
            var orderBy = "test";
            var select = "select";
            var skip = "21";
            var top = "15";

            return query;
        }
    }
}