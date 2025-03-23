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

            return query;
        }

        private static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, FilterParam filter)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            string[] properties = filter.Name.Split('.');
            Expression property = parameter;

            foreach (var prop in properties)
            {
                property = Expression.PropertyOrField(property, prop);
            }

            object convertedValue;
            try
            {
                var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
                convertedValue = Convert.ChangeType(filter.Value, propertyType);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Cannot convert '{filter.Value}' to type '{property.Type}'.", ex);
            }

            var constant = Expression.Constant(convertedValue, property.Type);
            Expression condition;

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

            // Nếu là danh sách, áp dụng Any()
            if (property.Type == typeof(IEnumerable<>).MakeGenericType(property.Type.GetGenericArguments()[0]))
            {
                var lambda = Expression.Lambda(condition, parameter);
                var anyMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(property.Type.GetGenericArguments()[0]);

                var anyCall = Expression.Call(anyMethod, property, lambda);
                var finalLambda = Expression.Lambda<Func<T, bool>>(anyCall, parameter);
                return query.Where(finalLambda);
            }
            else
            {
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                return query.Where(lambda);
            }
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