namespace Commerce.Command.Domain.Constants.Payment
{
    public class PaymentConst
    {
        // Table Names
        public const string TABLE_PAYMENT = "Payments";
        public const string TABLE_PAYMENT_DETAILS = "PaymentDetails";

        // Payments Table Fields
        public const string FIELD_PAYMENT_ID = "pay_Id";
        public const string FIELD_PAYMENT_ORDER_ID = "pay_OrderId";
        public const string FIELD_PAYMENT_AMOUNT = "pay_Amount";
        public const string FIELD_PAYMENT_URL = "pay_ReturnUrl";
        public const string FIELD_PAYMENT_ORDER_INFO = "pay_OrderInfo";
        public const string FIELD_PAYMENT_IPADDRESS = "pay_IPAddress";
        public const string FIELD_PAYMENT_TRANSACTION_ID = "pay_TransactionId";
        public const string FIELD_PAYMENT_METHOD = "pay_PaymentMethod";
        public const string FIELD_PAYMENT_DATE = "pay_PaymentDate";
        public const string FIELD_PAYMENT_RESPONSE_CODE = "pay_ResponseCode";
        public const string FIELD_PAYMENT_STATUS = "pay_PaymentStatus";
        public const string FIELD_PAYMENT_INSERTED_AT = "pay_InsertedAt";
        public const string FIELD_PAYMENT_INSERTED_BY = "pay_InsertedBy";
        public const string FIELD_PAYMENT_UPDATED_AT = "pay_UpdatedAt";
        public const string FIELD_PAYMENT_UPDATED_BY = "pay_UpdatedBy";
        public const string FIELD_PAYMENT_IS_DELETED = "pay_IsDeleted";

        // PaymentDetails Table Fields
        public const string FIELD_PAYMENT_DETAILS_ID = "pd_Id";
        public const string FIELD_PAYMENT_DETAILS_PAYMENT_ID = "pd_PaymentId";
        public const string FIELD_PAYMENT_DETAILS_TRANSACTION_CODE = "pd_BankCode";
        public const string FIELD_PAYMENT_DETAILS_BANK_NAME = "pd_BankName";
        public const string FIELD_PAYMENT_DETAILS_CARD_NUMBER = "pd_CardNumber";
        public const string FIELD_PAYMENT_DETAILS_NOTE = "pd_Note";
        public const string FIELD_PAYMENT_DETAILS_EXTRA_DATA = "pd_ExtraData";

        // Max Length Constraints
        public const int PAYMENT_TRANSACTION_ID_MAX_LENGTH = 128;
        public const int PAYMENT_METHOD_MAX_LENGTH = 50;
        public const int PAYMENT_STATUS_MAX_LENGTH = 50;
        public const int PAYMENT_DETAILS_TRANSACTION_CODE_MAX_LENGTH = 128;
        public const int PAYMENT_DETAILS_BANK_NAME_MAX_LENGTH = 128;
        public const int PAYMENT_DETAILS_CARD_NUMBER_MAX_LENGTH = 50;
        public const int PAYMENT_DETAILS_NOTE_MAX_LENGTH = 256;

        // Error Messages
        public const string INVALID_GUID = "{0} is an invalid GUID.";
        public const string MSG_PAYMENT_FK_CANT_DEL = $"{nameof(Payment)} cannot be deleted because it is referenced by other entities.";

    }
}
