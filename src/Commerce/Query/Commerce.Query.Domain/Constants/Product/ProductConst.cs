namespace Commerce.Query.Domain.Constants.Product
{
    public class ProductConst
    {
        public const string TABLE_PRODUCT = "Product";
        public const string FIELD_PRODUCT_ID = "prod_Id";
        public const string FIELD_PRODUCT_NAME = "prod_Name";
        public const string FIELD_PRODUCT_SIZE = "prod_Size";
        public const string FIELD_PRODUCT_COLOR = "prod_Color";
        public const string FIELD_PRODUCT_VIEW = "prod_Views";
        public const string FIELD_PRODUCT_DESCRIPTION = "prod_Description";
        public const string FIELD_PRODUCT_PRICE = "prod_Price";
        public const string FIELD_PRODUCT_CATEGORY_ID = "prod_CategoryId";
        public const string FIELD_PRODUCT_BRAND_ID = "prod_BrandId";
        public const string FIELD_PRODUCT_SHOP_ID = "prod_ShopId";
        public const string FIELD_PRODUCT_IMAGE = "prod_Image";
        public const string FIELD_PRODUCT_INSERTED_AT = "prod_InsertedAt";
        public const string FIELD_PRODUCT_INSERTED_BY = "prod_InsertedBy";
        public const string FIELD_PRODUCT_UPDATED_AT = "prod_UpdatedAt";
        public const string FIELD_PRODUCT_UPDATED_BY = "prod_UpdatedBy";
        public const string FIELD_PRODUCT_IS_DELETED = "prod_IsDeleted";

        public const string TABLE_PRODUCT_REVIEW = "ProductReview";
        public const string FIELD_PRODUCT_REVIEW_ID = "pp_Id";
        public const string FIELD_PRODUCT_REVIEW_PRODUCT_ID = "pp_ProductId";
        public const string FIELD_PRODUCT_REVIEW_USER_ID = "pp_UserId";
        public const string FIELD_PRODUCT_REVIEW_RATING = "pp_Rating";
        public const string FIELD_PRODUCT_REVIEW_COMMENT = "pp_Comment";
        public const string FIELD_PRODUCT_REVIEW_IMAGE = "pp_Image";
        public const string FIELD_PRODUCT_REVIEW_INSERTED_AT = "pp_InsertedAt";
        public const string FIELD_PRODUCT_REVIEW_INSERTED_BY = "pp_InsertedBy";
        public const string FIELD_PRODUCT_REVIEW_UPDATED_AT = "pp_UpdatedAt";
        public const string FIELD_PRODUCT_REVIEW_UPDATED_BY = "pp_UpdatedBy";
        public const string FIELD_PRODUCT_REVIEW_IS_DELETED = "pp_IsDeleted";

        public const string TABLE_PRODUCT_DETAIL = "ProductDetail";
        public const string FIELD_PRODUCT_DETAIL_ID = "prodDetail_Id";
        public const string FIELD_PRODUCT_DETAIL_QUANTITY = "prod_StockQuantity";

        public const int PRODUCT_REVIEW_COMMENT_MAX_LENGTH = 256;

        public const int PRODUCT_NAME_MAX_LENGTH = 128;
        public const int PRODUCT_SIZE_MAX_LENGTH = 10;
        public const int PRODUCT_COLOR_MAX_LENGTH = 32;

        public const int PRODUCT_DESCRIPTION_MAX_LENGTH = 256;
        public const int PRODUCT_IMAGE_MAX_LENGTH = 128;
        public const int PRODUCT_PRICE_MAX_DIGITS = 18;
        public const int PRODUCT_PRICE_DECIMALS = 2;

        public const string INVALID_GUID = "{0} is invalid guid.";

        public const string TABLE_PRODUCT_REVIEW_REPLY = "ProductReviewReply"; 
        public const string FIELD_PRODUCT_REVIEW_REPLY_ID = "prr_Id";
        public const string FIELD_PRODUCT_REVIEW_REPLY_REVIEW_ID = "prr_ReviewId";
        public const string FIELD_PRODUCT_REVIEW_REPLY_SHOP_ID = "prr_ShopId";
        public const string FIELD_PRODUCT_REVIEW_REPLY_REPLY = "prr_Reply";
        public const string FIELD_PRODUCT_REVIEW_REPLY_INSERTED_AT = "prr_InsertedAt";
        public const string FIELD_PRODUCT_REVIEW_REPLY_INSERTED_BY = "prr_InsertedBy";
        public const string FIELD_PRODUCT_REVIEW_REPLY_UPDATED_AT = "prr_UpdatedAt";
        public const string FIELD_PRODUCT_REVIEW_REPLY_UPDATED_BY = "prr_UpdatedBy";
        public const string FIELD_PRODUCT_REVIEW_REPLY_IS_DELETED = "prr_IsDeleted";
    }
}
