using System.Text.RegularExpressions;
using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;

namespace Commerce.Command.Contract.DependencyInjection.Extensions
{
    public static class RuleBuilderForStringExtensions
    {     
        /// <summary>
        /// Create rule that this property must be valid email
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<Guid?> IsGuid(this RuleBuilder<Guid?> ruleBuilder, string? message = null)
        {
            var msgArgs = new List<MessageArgs> { new(Args.PROPERTY_NAME, ruleBuilder.PropertyName) };
            message ??= MessConst.INVALID_GUID.FillArgs(msgArgs);

            // Tạo rule kiểm tra giá trị Guid hợp lệ
            bool Func(Guid? value) => value.HasValue && value != Guid.Empty;

            // Thêm rule thông qua phương thức công khai
            ruleBuilder.AddRule(new Rule<Guid?>(Func, ruleBuilder.Property, message));

            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be valid email
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> IsEmail(this RuleBuilder<string> ruleBuilder, string? message = null)
        {
            var msgArgs = new List<MessageArgs> { new(Args.PROPERTY_NAME, ruleBuilder.PropertyName) };
            message ??= MessConst.INVALID_EMAIL.FillArgs(msgArgs);

            // Regex check key must be alphabetical and not have any whitespace, special characters
            ruleBuilder.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", message);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be valid email
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> IsPhoneNumber(this RuleBuilder<string> ruleBuilder, string? message = null)
        {
            var msgArgs = new List<MessageArgs> { new(Args.PROPERTY_NAME, ruleBuilder.PropertyName) };
            message ??= MessConst.INVALID_PHONE.FillArgs(msgArgs);

            // Regex check key must be alphabetical and not have any whitespace, special characters
            ruleBuilder.Matches(@"^\+?\d{10,15}$", message);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must be valid email
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> NotVietnamese(this RuleBuilder<string> ruleBuilder, string? message = null)
        {
            var msgArgs = new List<MessageArgs> { new(Args.PROPERTY_NAME, ruleBuilder.PropertyName) };
            message ??= MessConst.INVALID_VN.FillArgs(msgArgs);

            // Regex check key must be alphabetical and not have any whitespace, special characters
            ruleBuilder.Matches(@"^[\x00-\x7F]*$", message);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must match a pattern
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="regexPattern">Regex pattern</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> Matches(this RuleBuilder<string> ruleBuilder,
                                                  string regexPattern,
                                                  string? message = null)
        {
            // Regex check key must be alphabetical and not have any whitespace, special characters
            var property = ruleBuilder.Property;
            if (property is null) return ruleBuilder;
            var match = Regex.Match(property, regexPattern);
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, regexPattern)
            };
            message ??= MessConst.NOT_MATCH.FillArgs(msgArgs);
            var rule = new Rule<string>(x => match.Success, property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Create rule that this property must not null or empty
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> NotNullOrEmpty(this RuleBuilder<string> ruleBuilder, string? message = null)
        {
            var property = ruleBuilder.Property;
            var msgArgs = new List<MessageArgs> { new(Args.PROPERTY_NAME, ruleBuilder.PropertyName) };
            message ??= MessConst.NOT_NULL_OR_EMPTY.FillArgs(msgArgs);
            Func<string, bool> validationFunc =
                x => !string.IsNullOrEmpty(property) && !string.IsNullOrWhiteSpace(property);
            var rule = new Rule<string>(validationFunc, property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        ///  Create rule that this property length must not exceed a specific maximum value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="maxLength">Max length</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> MaxLength(this RuleBuilder<string> ruleBuilder,
                                                    int maxLength,
                                                    string? message = null)
        {
            var property = ruleBuilder.Property;
            if (property is null) return ruleBuilder;
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, maxLength + MessConst.CHARACTERS)
            };
            message ??= MessConst.NOT_EXCEED.FillArgs(msgArgs);
            var rule = new Rule<string>(x => x?.Length <= maxLength, property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        ///  Create rule that this property length must not less a specific minimum value
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="minLength">Min length</param>
        /// <param name="message">Message attached. If not, will use default message</param>
        /// <returns></returns>
        public static RuleBuilder<string> MinLength(this RuleBuilder<string> ruleBuilder,
                                                    int minLength,
                                                    string? message = null)
        {
            var property = ruleBuilder.Property;
            if (property is null) return ruleBuilder;
            var msgArgs = new List<MessageArgs>
            {
                new(Args.PROPERTY_NAME, ruleBuilder.PropertyName),
                new(Args.COMPARISION_VALUE, minLength + MessConst.CHARACTERS)
            };
            message ??= MessConst.NOT_LESS_THAN.FillArgs(msgArgs);
            var rule = new Rule<string>(x => x?.Length >= minLength, property, message);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }
    }
}