namespace Commerce.Command.Domain.Constants.Order
{
    public class OrderConst
    {
        public const string TABLE_ORDER = "Order";
        public const string TABLE_ORDER_ITEM = "OrderItem";
        public const string TABLE_ORDER_CANCEL = "OrderCancellation";

        public const string FIELD_ORDER_ID = "od_Id";
        public const string FIELD_ORDER_USER_ID = "od_UserId";
        public const string FIELD_ORDER_PROMOTION_ID = "od_PromotionId";
        public const string FIELD_ORDER_TOTAL_AMOUNT = "od_TotalAmount";
        public const string FIELD_ORDER_PAYMENT_METHOD = "od_PaymentMethod";
        public const string FIELD_ORDER_ADDRESS = "od_Address";
        public const string FIELD_ORDER_STATUS = "od_Status";
        public const string FIELD_ORDER_INSERTED_AT = "od_InsertedAt";
        public const string FIELD_ORDER_INSERTED_BY = "od_InsertedBy";
        public const string FIELD_ORDER_UPDATED_AT = "od_UpdatedAt";
        public const string FIELD_ORDER_UPDATED_BY = "od_UpdatedBy";
        public const string FIELD_ORDER_IS_DELETED = "od_IsDeleted";

        public const string FIELD_ORDER_ITEM_ID = "oi_Id";
        public const string FIELD_ORDER_ITEM_ORDER_ID = "oi_OrderId";
        public const string FIELD_ORDER_ITEM_PRODUCT_ID = "oi_ProductDetailId";
        public const string FIELD_ORDER_ITEM_QUANTITY = "oi_Quantity";
        public const string FIELD_ORDER_ITEM_PRICE = "oi_Price";
        public const string FIELD_ORDER_ITEM_TOTAL = "oi_Total";

        public const string FIELD_ORDER_CANCEL_ID = "oc_Id"; 
        public const string FIELD_ORDER_CANCEL_ORDER_ID = "oc_OrderId"; 
        public const string FIELD_ORDER_CANCEL_REASON = "oc_Reason"; 
        public const string FIELD_ORDER_CANCEL_REFUND_AMOUNT = "oc_RefundAmount"; 
        public const string FIELD_ORDER_CANCEL_REFUND_STATUS = "oc_RefundStatus"; 
        public const string FIELD_ORDER_CANCEL_IS_REFUNDED = "oc_IsRefunded"; 
        public const string FIELD_ORDER_CANCEL_INSERTED_AT = "oc_InsertedAt";
        public const string FIELD_ORDER_CANCEL_INSERTED_BY = "oc_InsertedBy"; 
        public const string FIELD_ORDER_CANCEL_UPDATED_AT = "oc_UpdatedAt"; 
        public const string FIELD_ORDER_CANCEL_UPDATED_BY = "oc_UpdatedBy"; 
        public const string FIELD_ORDER_CANCEL_IS_DELETED = "oc_IsDeleted"; 

        public const int ORDER_PAYMENT_METHOD_MAX_LENGTH = 50;
        public const int ORDER_STATUS_MAX_LENGTH = 50;
        public const int ORDER_CANCEL_REASON_MAX_LENGTH = 255;

        public const string INVALID_GUID = "{0} is invalid guid.";
        public const string MSG_ORDER_FK_CANT_DEL = $"{nameof(Order)} can't be deleted because it's has been referenced by other entities";
    }
}
