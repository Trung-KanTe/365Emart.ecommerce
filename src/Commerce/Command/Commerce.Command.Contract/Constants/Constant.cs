namespace Commerce.Command.Contract.Contants
{
    /// <summary>
    /// Contain constant for arguments
    /// </summary>
    public static class Args
    {
        public const string PROPERTY_NAME = "{PropertyName}";
        public const string COMPARISION_VALUE = "{ComparisonValue}";
        public const string MIN_VALUE = "{MinValue}";
        public const string MAX_VALUE = "{MaxValue}";
        public const string COMPARISION_PROPERTY = "{ComparisonProperty}";
        public const string PROPERTY_VALUE = "{PropertyValue}";
        public const string PROPERTY_PATH = "{PropertyPath}";
        public const string TABLE_NAME = "{TableName}";
    }

    /// <summary>
    /// Contain constant for application message
    /// </summary>
    public class MessConst
    {
        public const string CHARACTERS = " characters";
        public const string NOT_FOUND = $"{Args.TABLE_NAME} was not found.";
        public const string NOT_NULL = $"{Args.PROPERTY_NAME} can't be null.";
        public const string NOT_EMPTY = $"{Args.PROPERTY_NAME} can't be empty.";
        public const string NOT_MATCH_CONDITION = $"{Args.PROPERTY_NAME} not match condition.";
        public const string NOT_NULL_OR_EMPTY = $"{Args.PROPERTY_NAME} can't be null or empty.";
        public const string NOT_EXCEED = $"{Args.PROPERTY_NAME} can't be exceed {Args.COMPARISION_VALUE}";
        public const string NOT_LESS_THAN = $"{Args.PROPERTY_NAME} can't be less than {Args.COMPARISION_VALUE}";
        public const string NOT_MATCH = $"{Args.PROPERTY_NAME} not match {Args.COMPARISION_VALUE}";
        public const string NULL_VALUE = "Null value was provided";
        public const string REQUIRED = $"{Args.PROPERTY_NAME} is required.";
        public const string TOO_LONG = $"{Args.PROPERTY_NAME} too long.";

        public const string MUST_EXCLUSIVE_FROM =
            $"{Args.PROPERTY_NAME} must be exclusive from {Args.MIN_VALUE} and {Args.MAX_VALUE}.";

        public const string MUST_INCLUSIVE_FROM =
            $"{Args.PROPERTY_NAME} must be inclusive from {Args.MIN_VALUE} and {Args.MAX_VALUE}.";

        public const string GREATER_THAN_OR_EQUAL = $"{Args.PROPERTY_NAME} must be greater than or equal to {Args.COMPARISION_VALUE}.";

        public const string GREATER_THAN = $"{Args.PROPERTY_NAME} must greater than {Args.COMPARISION_VALUE}.";
        public const string INVALID_FORMAT = $"{Args.PROPERTY_NAME} is invalid format";
        public const string INVALID_GUID = $"{Args.PROPERTY_NAME} is invalid guid";
        public const string INVALID_EMAIL = $"{Args.PROPERTY_NAME} is invalid email";
        public const string INVALID_PHONE = $"{Args.PROPERTY_NAME} is invalid phone";
        public const string INVALID_VN = $"{Args.PROPERTY_NAME} must not contains vietnamese characters";
        public const string NOT_NULL_OR_WHITE_SPACE = $"{Args.PROPERTY_NAME} must not be null or whitespaces.";
        public const string PROPERTY_ALREADY_EXIST = $"{Args.PROPERTY_NAME} already exist";
        public const string MSG_LOGIN = "Email or Password was not correct";
    }

    /// <summary>
    /// Contain constant for error code
    /// </summary>
    public static class ErrCodeConst
    {
        public const string CONFLICT = "CONFLICT";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string VALIDATION_PROBLEM = "VALIDATION_PROBLEM";
        public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
    }
}