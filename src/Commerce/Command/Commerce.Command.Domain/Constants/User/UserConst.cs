namespace Commerce.Command.Domain.Constants.User
{
    public class UserConst
    {
        public const string TABLE_USER = "User";
        public const string FIELD_USER_ID = "u_Id";
        public const string FIELD_USER_NAME = "u_Name";
        public const string FIELD_USER_EMAIL = "u_Email";
        public const string FIELD_USER_PASSWORD_HASH = "u_PasswordHash";
        public const string FIELD_USER_TEL = "u_Tel";
        public const string FIELD_USER_ADDRESS = "u_Address";
        public const string FIELD_USER_WARD_ID = "u_WardId";
        public const string FIELD_USER_INSERTED_AT = "u_InsertedAt";
        public const string FIELD_USER_INSERTED_BY = "u_InsertedBy";
        public const string FIELD_USER_UPDATED_AT = "u_UpdatedAt";
        public const string FIELD_USER_UPDATED_BY = "u_UpdatedBy";
        public const string FIELD_USER_IS_DELETED = "u_IsDeleted";

        public const int USER_NAME_MAX_LENGTH = 128;
        public const int USER_EMAIL_MAX_LENGTH = 128;
        public const int USER_PASSWORD_HASH_MAX_LENGTH = 256;
        public const int USER_TEL_MAX_LENGTH = 15;
        public const int USER_ADDRESS_MAX_LENGTH = 256;

        public const string TABLE_ROLE = "Roles";
        public const string FIELD_ROLE_ID = "role_Id";
        public const string FIELD_ROLE_NAME = "role_Name";
        public const string FIELD_ROLE_DESCRIPTION = "role_Description";

        public const int ROLE_NAME_MAX_LENGTH = 128;
        public const int ROLE_DESCRIPTION_MAX_LENGTH = 256;

        public const string TABLE_USER_ROLE = "UserRoles";
        public const string FIELD_USER_ROLE_USER_ID = "u_Id";
        public const string FIELD_USER_ROLE_ROLE_ID = "role_Id";

        public const string INVALID_GUID = "{0} is invalid guid.";
        public const string MSG_LOGIN = "Email or Password was not correct";
        public const string MSG_USER_ID_FK = $"Can't deleted {nameof(User)} because it's referenced by other entity";
        public const string MSG_USER_ID_NOT_FOUND = "{0} was not found find by key.";
    }
}
