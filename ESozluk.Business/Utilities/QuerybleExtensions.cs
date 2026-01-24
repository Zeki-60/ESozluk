using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Utilities
{
    public static class QuerybleExtensions
    {
        public static IQueryable<T> ToPage<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
