using System.Linq.Expressions;
using System.Reflection;

namespace Commerce.Query.Contract.DependencyInjection.Extensions
{
    /// <summary>
    /// Extensions for "Expression" class
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Get property name as string from expression 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string? GetPropertyName(this Expression expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                return propertyInfo.Name;
            }

            return null;
        }

        /// <summary>
        /// Map data from source entity to target entity
        /// </summary>
        /// <typeparam name="TTarget">Destination entity, which data of source entity will be mapped to</typeparam>
        /// <param name="source">Current entity</param>
        /// <param name="ignoreNull">If true, null value of source properties will not be mapped to target and keep original data, otherwise, map null to target property</param>
        /// <returns>New value of target</returns>
        public static TTarget? MapTo<TTarget>(this object source, bool ignoreNull = false) where TTarget : class?, new()
        {
            var target = new TTarget();
            return MapTo(source, target, ignoreNull);
        }

        /// <summary>
        /// Map data from source entity to target entity
        /// </summary>
        /// <typeparam name="TSource">Current entity</typeparam>
        /// <typeparam name="TTarget">Destination entity, which data of source entity will be mapped to</typeparam>
        /// <param name="source">Current entity</param>
        /// <param name="target">Destination entity, which data of source entity will be mapped to</param>
        /// <param name="ignoreNull">If true, null value of source will be mapped to target, otherwise, keep target original data</param>
        /// <returns>New value of target</returns>
        public static TTarget? MapTo<TSource, TTarget>(this TSource source, TTarget target, bool ignoreNull = false) where TSource : class? where TTarget : class?, new()
        {
            if (source is null) return null;

            // Create new target when target is null
            target ??= new TTarget();

            // Get source properties
            var sourceProperties = source.GetType().GetProperties();

            // Get target properties
            var targetProperties = target.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                // Find matched property name and type of source and target
                var targetProperty = Array.Find(targetProperties, p => p.Name == sourceProperty.Name);

                if (targetProperty != null && targetProperty.CanWrite)
                {
                    // Get source property value
                    var value = sourceProperty.GetValue(source);

                    // If ignore null, will keep original target data
                    if (ignoreNull && value == null)
                        value = targetProperty.GetValue(target);

                    try
                    {
                        // Handling type conversion for Guid mapping from string
                        if (targetProperty.PropertyType == typeof(Guid) || targetProperty.PropertyType == typeof(Guid?))
                        {
                            if (value is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
                            {
                                value = parsedGuid;
                            }
                            // Handle the case when value is null
                            else if (value == null && targetProperty.PropertyType == typeof(Guid?))
                            {
                                value = null; // Allow nullable Guid to be null
                            }
                        }

                        // Mapping from source to target
                        targetProperty.SetValue(target, value);
                    }
                    catch (Exception)
                    {
                        //var message = MsgConst.UN_SUP_MAP.FormatMsg(sourceProperty.Name, sourceProperty.PropertyType, targetProperty.PropertyType);
                        //throw new CustomException()
                        //    .WithMessageCode(MsgCode.ERR_INTERNAL_SERVER)
                        //    .WithDetails(message)
                        //    .WithStatusCode((int)HttpStatusCode.InternalServerError);
                        continue;
                    }
                }
            }

            return target;
        }
    }
}