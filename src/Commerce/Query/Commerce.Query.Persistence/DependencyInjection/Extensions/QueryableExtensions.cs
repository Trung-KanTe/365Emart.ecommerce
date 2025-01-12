using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Commerce.Query.Persistence.DependencyInjection.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Extension method of IQueryable for including multiple relationship
        /// </summary>
        /// <typeparam name="TEntity">Type of domain entity</typeparam>
        /// <param name="source">IQueryable source need to including properties</param>
        /// <param name="includeProperties">Properties to be included</param>
        /// <returns>IQueryable with included properties</returns>
        public static IQueryable<TEntity> IncludeMultiple<TEntity>(this IQueryable<TEntity> source,
                                                                   params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : class
        {
            if (includeProperties.Any())
            {
                // Each property will be included into source
                source = includeProperties.Aggregate(source, (current, include) => current.Include(include));
            }

            return source;
        }
    }
}