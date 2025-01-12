namespace Commerce.Command.Contract.Enumerations
{
    /// <summary>
    /// Status code of application action result
    /// </summary>
    public class StatusCode
    {
        public const int NotFound = 404;
        public const int Success = 200;
        public const int BadRequest = 400;
        public const int Created = 201;
        public const int Ok = 200;
        public const int InternalServerError = 500;
        public const int Conflict = 409;
    }
}