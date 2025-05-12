namespace Commerce.Query.Domain.Constants.Wallets
{
    public class WalletConst
    {
        // === TABLE NAMES ===
        public const string TABLE_SHOP_WALLET = "ShopWallet";
        public const string TABLE_SHOP_WALLET_TRANSACTION = "ShopWalletTransaction";
        public const string TABLE_PLATFORM_WALLET = "PlatformWallet";
        public const string TABLE_PLATFORM_WALLET_TRANSACTION = "PlatformWalletTransaction";

        // === ShopWallet Fields ===
        public const string FIELD_SHOP_WALLET_ID = "sw_Id";
        public const string FIELD_SHOP_WALLET_SHOP_ID = "sw_ShopId";
        public const string FIELD_SHOP_WALLET_BALANCE = "sw_Balance";
        public const string FIELD_SHOP_WALLET_INSERTED_AT = "sw_InsertedAt";
        public const string FIELD_SHOP_WALLET_INSERTED_BY = "sw_InsertedBy";
        public const string FIELD_SHOP_WALLET_UPDATED_AT = "sw_UpdatedAt";
        public const string FIELD_SHOP_WALLET_UPDATED_BY = "sw_UpdatedBy";

        // === ShopWalletTransaction Fields ===
        public const string FIELD_SHOP_WALLET_TRANS_ID = "swt_Id";
        public const string FIELD_SHOP_WALLET_TRANS_WALLET_ID = "swt_ShopWalletId";
        public const string FIELD_SHOP_WALLET_TRANS_ORDER_ID = "swt_OrderId";
        public const string FIELD_SHOP_WALLET_TRANS_AMOUNT = "swt_Amount";
        public const string FIELD_SHOP_WALLET_TRANS_TYPE = "swt_Type";
        public const string FIELD_SHOP_WALLET_TRANS_DESCRIPTION = "swt_Description";
        public const string FIELD_SHOP_WALLET_TRANS_CREATED_AT = "swt_CreatedAt";

        // === PlatformWallet Fields ===
        public const string FIELD_PLATFORM_WALLET_ID = "pw_Id";
        public const string FIELD_PLATFORM_WALLET_BALANCE = "pw_Balance";
        public const string FIELD_PLATFORM_WALLET_INSERTED_AT = "pw_InsertedAt";
        public const string FIELD_PLATFORM_WALLET_INSERTED_BY = "pw_InsertedBy";
        public const string FIELD_PLATFORM_WALLET_UPDATED_AT = "pw_UpdatedAt";
        public const string FIELD_PLATFORM_WALLET_UPDATED_BY = "pw_UpdatedBy";

        // === PlatformWalletTransaction Fields ===
        public const string FIELD_PLATFORM_WALLET_TRANS_ID = "pwt_Id";
        public const string FIELD_PLATFORM_WALLET_TRANS_ORDER_ID = "pwt_OrderId";
        public const string FIELD_PLATFORM_WALLET_TRANS_AMOUNT = "pwt_Amount";
        public const string FIELD_PLATFORM_WALLET_TRANS_TYPE = "pwt_Type";
        public const string FIELD_PLATFORM_WALLET_TRANS_DESCRIPTION = "pwt_Description";
        public const string FIELD_PLATFORM_WALLET_TRANS_CREATED_AT = "pwt_CreatedAt";

        // === Max Length Constraints ===
        public const int WALLET_TRANS_TYPE_MAX_LENGTH = 50;
        public const int WALLET_TRANS_DESCRIPTION_MAX_LENGTH = 256;

        // === Error Messages ===
        public const string INVALID_GUID = "{0} is invalid guid.";
        public const string MSG_SHOP_WALLET_FK_CANT_DEL = "ShopWallet can't be deleted because it has related transactions.";
        public const string MSG_PLATFORM_WALLET_FK_CANT_DEL = "PlatformWallet can't be deleted because it has related transactions.";
    }
}
