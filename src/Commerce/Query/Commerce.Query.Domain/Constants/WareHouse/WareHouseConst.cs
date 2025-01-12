namespace Commerce.Query.Domain.Constants.WareHouse
{
    public class WareHouseConst
    {
        public const string TABLE_WAREHOUSE = "WareHouse";

        // Tên trường (Field names)
        public const string FIELD_WAREHOUSE_ID = "wh_Id";
        public const string FIELD_WAREHOUSE_NAME = "wh_Name";
        public const string FIELD_WAREHOUSE_ADDRESS = "wh_Address";
        public const string FIELD_WAREHOUSE_WARD_ID = "wh_WardId";
        public const string FIELD_WAREHOUSE_INSERTED_AT = "wh_InsertedAt";
        public const string FIELD_WAREHOUSE_INSERTED_BY = "wh_InsertedBy";
        public const string FIELD_WAREHOUSE_UPDATED_AT = "wh_UpdatedAt";
        public const string FIELD_WAREHOUSE_UPDATED_BY = "wh_UpdatedBy";
        public const string FIELD_WAREHOUSE_IS_DELETED = "wh_IsDeleted";

        // Giới hạn độ dài của các trường
        public const int WAREHOUSE_NAME_MAX_LENGTH = 128;
        public const int WAREHOUSE_ADDRESS_MAX_LENGTH = 256;

        // Thông báo lỗi
        public const string INVALID_GUID = "{0} is invalid guid.";
    }
}
