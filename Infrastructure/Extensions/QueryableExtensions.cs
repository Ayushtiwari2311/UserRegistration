using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string sortColumn, bool ascending)
        {
            var parameter = Expression.Parameter(typeof(T), "e");

            Expression propertyAccess = parameter;

            foreach (var member in sortColumn.Split('.'))
            {
                propertyAccess = Expression.PropertyOrField(propertyAccess, member);
            }

            var lambda = Expression.Lambda(propertyAccess, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), propertyAccess.Type)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result!;
        }
    }
}
