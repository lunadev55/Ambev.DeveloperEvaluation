using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Extensions
{
    /// <summary>
    /// Provides dynamic ordering support for IQueryable<T> by property name.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Orders the source by the specified property name and direction.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="source">Queryable source</param>
        /// <param name="propertyName">Property name to order by</param>
        /// <param name="descending">True for descending order, false for ascending</param>
        /// <returns>Ordered query</returns>
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(selector, parameter);

            string methodName = descending ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), selector.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
