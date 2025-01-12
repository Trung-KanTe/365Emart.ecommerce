namespace Commerce.Query.Domain.Constants.Brand
{
    /// <summary>
    /// Contain constants for Commerce
    /// </summary>
    public class BrandConst
    {
        public const string TABLE_BRAND = "Brand";
        public const string FIELD_ID = "brd_Id";
        public const string FIELD_NAME = "brd_Name";
        public const string FIELD_ICON = "brd_Icon";
        public const string FIELD_STYLE = "brd_Style";
        public const string FIELD_USER_ID = "brd_UserId";
        public const string FIELD_VIEWS = "brd_Views";
        public const string FIELD_INSERTED_AT = "brd_InsertedAt";
        public const string FIELD_INSERTED_BY = "brd_InsertedBy";
        public const string FIELD_UPDATED_AT = "brd_UpdatedAt";
        public const string FIELD_UPDATED_BY = "brd_UpdatedBy";
        public const string FIELD_DELETE = "brd_IsDeleted";

        public const int NAME_MAX_LENGTH = 128;
        public const int ICON_MAX_LENGTH = 128;
        public const int STYLE_MAX_LENGTH = 128;
    }
}