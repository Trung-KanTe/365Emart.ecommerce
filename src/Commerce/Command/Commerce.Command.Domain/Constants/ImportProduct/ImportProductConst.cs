namespace Commerce.Command.Domain.Constants.ImportProduct
{
    public class ImportProductConst
    {
        public const string TABLE_IMPORT_PRODUCT = "ImportProduct";
        public const string TABLE_IMPORT_PRODUCT_DETAILS = "ImportProductDetails";
        public const string TABLE_PRODUCT_STOCK = "ProductStock";

        public const string FIELD_IMPORT_PRODUCT_ID = "ip_Id";
        public const string FIELD_IMPORT_PRODUCT_PARTNER_ID = "ip_PartnerId";
        public const string FIELD_IMPORT_PRODUCT_SHOP_ID = "ip_ShopId";
        public const string FIELD_IMPORT_PRODUCT_WAREHOUSE_ID = "ip_WareHouseId";
        public const string FIELD_IMPORT_PRODUCT_IMPORT_DATE = "ip_ImportDate";
        public const string FIELD_IMPORT_PRODUCT_NOTE = "ip_Note";
        public const string FIELD_IMPORT_PRODUCT_INSERTED_AT = "ip_InsertedAt";
        public const string FIELD_IMPORT_PRODUCT_INSERTED_BY = "ip_InsertedBy";
        public const string FIELD_IMPORT_PRODUCT_UPDATED_AT = "ip_UpdatedAt";
        public const string FIELD_IMPORT_PRODUCT_UPDATED_BY = "ip_UpdatedBy";
        public const string FIELD_IMPORT_PRODUCT_IS_DELETED = "ip_IsDeleted";

        public const string FIELD_IMPORT_PRODUCT_DETAILS_ID = "ipd_Id";
        public const string FIELD_IMPORT_PRODUCT_DETAILS_PRODUCT_ID = "ipd_ProductDetailId";
        public const string FIELD_IMPORT_PRODUCT_DETAILS_IMPORT_PRODUCT_ID = "ipd_ImportProductId";
        public const string FIELD_IMPORT_PRODUCT_DETAILS_IMPORT_PRICE = "ipd_ImportPrice";
        public const string FIELD_IMPORT_PRODUCT_DETAILS_QUANTITY = "ipd_Quantity";
        public const string FIELD_IMPORT_PRODUCT_DETAILS_NOTE = "ipd_Note";

       
        public const string FIELD_PRODUCT_STOCK_ID = "ps_Id";
        public const string FIELD_PRODUCT_STOCK_PRODUCT_ID = "ps_ProductDetailId";
        public const string FIELD_PRODUCT_STOCK_QUANTITY = "ps_Quantity";
        public const string FIELD_PRODUCT_STOCK_WAREHOUSE_ID = "ps_WareHouseId";

        public const int IMPORT_PRODUCT_NOTE_MAX_LENGTH = 256;
        public const int IMPORT_PRODUCT_DETAILS_NOTE_MAX_LENGTH = 256;

        public const string INVALID_GUID = "{0} is invalid guid.";
        public const string MSG_IMPORT_FK_CANT_DEL = $"{nameof(ImportProduct)} can't be deleted because it's has been referenced by other entites";
    }
}