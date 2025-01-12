using System.Globalization;
using Commerce.Query.Contract.Contants;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;

namespace Commerce.Query.Contract.DependencyInjection.Extensions
{
    public static class RuleBuilderForDateTimeExtensions
    {
        /// <summary>
        /// Create rule that this property must be greater than a datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="value">Value to be compared</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> GreaterThan(this RuleBuilder<DateTime> ruleBuilder,
                                                        DateTime value,
                                                        string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, value.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.GREATER_THAN.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x > value, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be greater than or equal a datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="value">Value to be compared</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> GreaterThanOrEqual(this RuleBuilder<DateTime> ruleBuilder,
                                                               DateTime value,
                                                               string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, value.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.GREATER_THAN.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x >= value, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be lower than a datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="value">Value to be compared</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> LowerThan(this RuleBuilder<DateTime> ruleBuilder,
                                                      DateTime value,
                                                      string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, value.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.GREATER_THAN.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x < value, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be lower than or equal a datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="value">Value to be compared</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> LowerThanOrEqual(this RuleBuilder<DateTime> ruleBuilder,
                                                             DateTime value,
                                                             string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, value.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.GREATER_THAN.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x <= value, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be exclusive between 2 datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="minValue">First datetime value</param>
        /// <param name="maxValue">Second datetime value</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> ExclusiveBetween(this RuleBuilder<DateTime> ruleBuilder,
                                                             DateTime minValue,
                                                             DateTime maxValue,
                                                             string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.MIN_VALUE, minValue.ToString(CultureInfo.InvariantCulture)),
                new(Args.MAX_VALUE, maxValue.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.MUST_EXCLUSIVE_FROM.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x >= maxValue || x <= minValue, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be inclusive between 2 datetime value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="minValue">First datetime value</param>
        /// <param name="maxValue">Second datetime value</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<DateTime> InclusiveBetween(this RuleBuilder<DateTime> ruleBuilder,
                                                             DateTime minValue,
                                                             DateTime maxValue,
                                                             string? message = null)
        {
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.MIN_VALUE, minValue.ToString(CultureInfo.InvariantCulture)),
                new(Args.MAX_VALUE, maxValue.ToString(CultureInfo.InvariantCulture))
            };
            message = message ?? MessConst.MUST_INCLUSIVE_FROM.FillArgs(msgArgs);
            var rule = new Rule<DateTime>(x => x <= maxValue && x >= minValue, ruleBuilder.Property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }
    }
}