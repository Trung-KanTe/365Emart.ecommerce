namespace Commerce.Query.Contract.Validators
{
    /// <summary>
    /// Marker interface of validation rule
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Message for fail when validate
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Validate according to this rule
        /// </summary>
        /// <returns></returns>
        public bool Validate();
    }

    public class Rule<TProperty> : IRule
    {
        public string Message { get; }

        /// <summary>
        /// Function to validate
        /// </summary>
        private Func<TProperty, bool> validationFunc;

        /// <summary>
        /// Property to validate
        /// </summary>
        private TProperty property;

        public Rule(Func<TProperty, bool> validationFunc, TProperty property, string message)
        {
            this.property = property;
            this.validationFunc = validationFunc;
            Message = message;
        }

        /// <summary>
        /// Validate this property using validate function
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            var result = validationFunc.Invoke(property);
            return result;
        }
    }
}