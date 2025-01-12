using System.Linq.Expressions;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Exceptions;

namespace Commerce.Query.Contract.Validators
{
    /// <summary>
    /// Marker interface to define action of validator
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Add new rule to validate
        /// </summary>
        /// <param name="rule"></param>
        public void AddRule(IRule rule);
    }

    /// <summary>
    /// Validator for specific entity
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class Validator<TEntity> : IValidator
    {
        /// <summary>
        /// Rules to validate entity
        /// </summary>
        private readonly List<IRule> rules = new();

        /// <summary>
        /// Entity to validate
        /// </summary>
        private readonly TEntity entity;

        /// <summary>
        /// Validation result. If empty mean this entity not have any validation problem
        /// </summary>
        private readonly List<string> validationResults = new();

        /// <summary>
        /// Validate entity
        /// </summary>
        /// <exception cref="DomainValidationException">Throw when entity has validation problems</exception>
        public void Validate()
        {
            foreach (var rule in rules)
                if (!rule.Validate())
                    validationResults.Add(rule.Message);
            if (validationResults.Any()) throw new DomainValidationException(validationResults.ToArray());
        }

        public Validator(TEntity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// Create new validator for entity
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns></returns>
        public static Validator<TEntity> Create(TEntity entity)
        {
            return new Validator<TEntity>(entity);
        }

        /// <summary>
        /// Add new rule to validate
        /// </summary>
        /// <param name="rule"></param>
        public void AddRule(IRule rule)
        {
            rules.Add(rule);
        }

        /// <summary>
        /// Create RuleBuilder for specific property
        /// </summary>
        /// <typeparam name="TType">Type of property</typeparam>
        /// <param name="expression">Lambda expression to get property from entity</param>
        /// <returns></returns>
        public RuleBuilder<TType> RuleFor<TType>(Expression<Func<TEntity, TType?>> expression)
        {
            var func = expression.Compile();
            var ruleBuilder =
                new RuleBuilder<TType>(this, func.Invoke(entity)!, expression.GetPropertyName()!);
            return ruleBuilder;
        }
    }

    /// <summary>
    /// Helper class to create validator 
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// Create validator for entity
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="entity">Entity to validate</param>
        /// <returns></returns>
        public static Validator<TEntity> Create<TEntity>(TEntity entity)
        {
            return Validator<TEntity>.Create(entity);
        }
    }
}