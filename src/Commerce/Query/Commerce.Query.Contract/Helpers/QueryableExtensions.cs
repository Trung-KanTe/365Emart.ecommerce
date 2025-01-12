using System.Linq.Expressions;

namespace Commerce.Query.Contract.Helpers
{

    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, SearchCommand searchCommand)
        {
            // Apply Filters
            foreach (var filter in searchCommand.FilterParams)
            {
                query = query.ApplyFilter(filter);
            }

            // Apply Sorting
            foreach (var order in searchCommand.OrderParams)
            {
                query = query.ApplySorting(order);
            }

            // Apply Paging
            query = query.Skip((searchCommand.PageNumber - 1) * searchCommand.PageSize)
                         .Take(searchCommand.PageSize);

            return query;
        }

        private static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, FilterParam filter)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, filter.Name);

            // Chuyển đổi giá trị sang kiểu phù hợp
            object convertedValue;
            try
            {
                if (Nullable.GetUnderlyingType(property.Type) != null)
                {
                    // Kiểu nullable, chuyển đổi giá trị sang kiểu không nullable
                    var underlyingType = Nullable.GetUnderlyingType(property.Type);
                    convertedValue = string.IsNullOrEmpty(filter.Value)
                        ? null
                        : Convert.ChangeType(filter.Value, underlyingType);
                }
                else
                {
                    // Kiểu không nullable
                    convertedValue = Convert.ChangeType(filter.Value, property.Type);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Cannot convert '{filter.Value}' to type '{property.Type}'.", ex);
            }

            var constant = Expression.Constant(convertedValue, property.Type);
            Expression condition;

            // Áp dụng điều kiện
            switch (filter.Condition)
            {
                case "=":
                    condition = Expression.Equal(property, constant);
                    break;
                case ">":
                    condition = Expression.GreaterThan(property, constant);
                    break;
                case ">=":
                    condition = Expression.GreaterThanOrEqual(property, constant);
                    break;
                case "<":
                    condition = Expression.LessThan(property, constant);
                    break;
                case "<=":
                    condition = Expression.LessThanOrEqual(property, constant);
                    break;
                case "Contains":
                    condition = Expression.Call(property, "Contains", null, constant);
                    break;
                default:
                    throw new NotSupportedException($"Condition '{filter.Condition}' is not supported.");
            }

            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            return query.Where(lambda);
        }     

        private static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, OrderParam order)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, order.Name);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = order.OrderDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
        }
    }
}