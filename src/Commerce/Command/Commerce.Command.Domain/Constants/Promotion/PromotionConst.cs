namespace Commerce.Command.Domain.Constants.Promotion
{
    public class PromotionConst
    {
        public const string TABLE_PROMOTION = "Promotion";
        public const string FIELD_ID = "prm_Id";
        public const string FIELD_NAME = "prm_Name";
        public const string FIELD_DESCRIPTION = "prm_Description";
        public const string FIELD_DISCOUNT_TYPE = "prm_DiscountCode";
        public const string FIELD_DISCOUNT_VALUE = "prm_DiscountValue";
        public const string FIELD_START_DATE = "prm_StartDate";
        public const string FIELD_END_DATE = "prm_EndDate";
        public const string FIELD_INSERTED_AT = "prm_InsertedAt";
        public const string FIELD_INSERTED_BY = "prm_InsertedBy";
        public const string FIELD_UPDATED_AT = "prm_UpdatedAt";
        public const string FIELD_UPDATED_BY = "prm_UpdatedBy";
        public const string FIELD_DELETE = "prm_IsDeleted";

        public const int NAME_MAX_LENGTH = 128;
        public const int DESCRIPTION_MAX_LENGTH = 256;
        public const int DISCOUNT_TYPE_MAX_LENGTH = 64;

        public const string INVALID_GUID = "{0} is invalid guid.";
        public const string INVALID_DISCOUNT_VALUE = "Discount value must be greater than or equal to 0.";
        public const string INVALID_DATE_RANGE = "Start date must be earlier than end date.";
    }
}
