namespace Commerce.Query.Domain.Constants.Shop
{
    public class ShopConst
    {
        public const string TABLE_SHOP = "Shop";
        public const string FIELD_SHOP_ID = "sh_Id";
        public const string FIELD_SHOP_NAME = "sh_Name";
        public const string FIELD_SHOP_DESCRIPTION = "sh_Description";
        public const string FIELD_SHOP_IMAGE = "sh_Image";
        public const string FIELD_SHOP_STYLE = "sh_Style";
        public const string FIELD_SHOP_TEL = "sh_Tel";
        public const string FIELD_SHOP_EMAIL = "sh_Email";
        public const string FIELD_SHOP_WEBSITE = "sh_Website";
        public const string FIELD_SHOP_ADDRESS = "sh_Address";
        public const string FIELD_SHOP_WARD_ID = "sh_WardId";
        public const string FIELD_SHOP_USER_ID = "sh_UserId";
        public const string FIELD_SHOP_VIEWS = "sh_Views";
        public const string FIELD_SHOP_INSERTED_AT = "sh_InsertedAt";
        public const string FIELD_SHOP_INSERTED_BY = "sh_InsertedBy";
        public const string FIELD_SHOP_UPDATED_AT = "sh_UpdatedAt";
        public const string FIELD_SHOP_UPDATED_BY = "sh_UpdatedBy";
        public const string FIELD_SHOP_IS_DELETED = "sh_IsDeleted";

        public const int SHOP_NAME_MAX_LENGTH = 128;
        public const int SHOP_DESCRIPTION_MAX_LENGTH = 256;
        public const int SHOP_IMAGE_MAX_LENGTH = 128;
        public const int SHOP_STYLE_MAX_LENGTH = 128;
        public const int SHOP_TEL_MAX_LENGTH = 15;
        public const int SHOP_EMAIL_MAX_LENGTH = 256;
        public const int SHOP_ADDRESS_MAX_LENGTH = 256;
        public const int SHOP_WEBSITE_MAX_LENGTH = 256;

        public const string INVALID_GUID = "{0} is invalid guid.";
    }
}
