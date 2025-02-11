using Commerce.Query.Domain.Entities.Cart;

namespace Commerce.Query.Domain.Constants.Cart
{
    public class CartConst
    {
        public const string TABLE_SHOPPING_CART = "ShoppingCart";
        public const string TABLE_CART_ITEMS = "CartItems";

        // Fields for ShoppingCart
        public const string FIELD_CART_ID = "sc_Id";
        public const string FIELD_CART_USER_ID = "sc_UserId";
        public const string FIELD_CART_TOTAL_QUANTITY = "sc_TotalQuantity";
        public const string FIELD_CART_INSERTED_AT = "sc_InsertedAt";
        public const string FIELD_CART_INSERTED_BY = "sc_InsertedBy";
        public const string FIELD_CART_UPDATED_AT = "sc_UpdatedAt";
        public const string FIELD_CART_UPDATED_BY = "sc_UpdatedBy";
        public const string FIELD_CART_IS_DELETED = "sc_IsDeleted";

        // Fields for CartItems
        public const string FIELD_CART_ITEM_ID = "ci_Id";
        public const string FIELD_CART_ITEM_CART_ID = "ci_CartId";
        public const string FIELD_CART_ITEM_PRODUCT_ID = "ci_ProductDetailId";
        public const string FIELD_CART_ITEM_PRICE = "ci_Price";
        public const string FIELD_CART_ITEM_QUANTITY = "ci_Quantity";
        public const string FIELD_CART_ITEM_TOTAL = "ci_Total";

        // Max length for Notes (if applicable in the future)
        public const int CART_ITEM_NOTE_MAX_LENGTH = 256;

        // Error message for deleting a shopping cart that has items
        public const string MSG_CART_FK_CANT_DEL = $"{nameof(Cart)} can't be deleted because it has referenced {nameof(CartItem)}.";
    }
}
