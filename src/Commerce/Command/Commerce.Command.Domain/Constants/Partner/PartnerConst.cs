namespace Commerce.Command.Domain.Constants.Partner
{
    public class PartnerConst
    {
        public const string TABLE_PARTNER = "Partner";
        public const string FIELD_PARTNER_ID = "par_Id";
        public const string FIELD_PARTNER_NAME = "par_Name";
        public const string FIELD_PARTNER_DESCIPTION = "par_Description";
        public const string FIELD_PARTNER_ICON = "par_Icon";
        public const string FIELD_PARTNER_TEL = "par_Tel";
        public const string FIELD_PARTNER_WEBSITE = "par_Website";
        public const string FIELD_PARTNER_EMAIL = "par_Email";
        public const string FIELD_PARTNER_ADDRESS = "par_Address";
        public const string FIELD_PARTNER_WARD_ID = "par_WardId";
        public const string FIELD_PARTNER_INSERTED_AT = "par_InsertedAt";
        public const string FIELD_PARTNER_INSERTED_BY = "par_InsertedBy";
        public const string FIELD_PARTNER_UPDATED_AT = "par_UpdatedAt";
        public const string FIELD_PARTNER_UPDATED_BY = "par_UpdatedBy";
        public const string FIELD_PARTNER_IS_DELETED = "par_IsDeleted";

        public const int PARTNER_NAME_MAX_LENGTH = 128;
        public const int PARTNER_DESCIPTION_MAX_LENGTH = 256;
        public const int PARTNER_EMAIL_MAX_LENGTH = 128;
        public const int PARTNER_ICON_MAX_LENGTH = 128;
        public const int PARTNER_TEL_MAX_LENGTH = 15;
        public const int PARTNER_ADDRESS_MAX_LENGTH = 256;
        public const int PARTNER_WEBSITE_MAX_LENGTH = 256;

        public const string INVALID_GUID = "{0} is invalid guid.";
    }
}
