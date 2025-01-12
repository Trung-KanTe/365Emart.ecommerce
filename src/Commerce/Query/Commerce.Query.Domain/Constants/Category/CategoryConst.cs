namespace Commerce.Query.Domain.Constants.Category
{
    /// <summary>
    /// Contain constants for Category
    /// </summary>
    public class CategoryConst
    {
        public const string TABLE_CATEGORY = "Category";
        public const string TABLE_CLASSIFICATION_CATEGORY = "ClassificationCategory";
        public const string FIELD_CATEGORY_ID = "cat_Id";
        public const string FIELD_CATEGORY_NAME = "cat_Name";
        public const string FIELD_CATEGORY_IMAGE = "cat_Image";
        public const string FIELD_CATEGORY_STYLE = "cat_Style";
        public const string FIELD_CATEGORY_USER_ID = "cat_UserId";
        public const string FIELD_CATEGORY_VIEWS = "cat_Views";
        public const string FIELD_CATEGORY_INSERTED_AT = "cat_InsertedAt";
        public const string FIELD_CATEGORY_INSERTED_BY = "cat_InsertedBy";
        public const string FIELD_CATEGORY_UPDATED_AT = "cat_UpdatedAt";
        public const string FIELD_CATEGORY_UPDATED_BY = "cat_UpdatedBy";
        public const string FIELD_CATEGORY_IS_DELETED = "cat_IsDeleted";

        public const string TABLE_CLASSIFICATION = "Classification";
        public const string FIELD_CLASSIFICATION_ID = "class_Id";
        public const string FIELD_CLASSIFICATION_NAME = "class_Name";
        public const string FIELD_CLASSIFICATION_ICON = "class_Icon";
        public const string FIELD_CLASSIFICATION_STYLE = "class_Style";
        public const string FIELD_CLASSIFICATION_VIEWS = "class_Views";
        public const string FIELD_CLASSIFICATION_INSERTED_AT = "class_InsertedAt";
        public const string FIELD_CLASSIFICATION_INSERTED_BY = "class_InsertedBy";
        public const string FIELD_CLASSIFICATION_UPDATED_AT = "class_UpdatedAt";
        public const string FIELD_CLASSIFICATION_UPDATED_BY = "class_UpdatedBy";
        public const string FIELD_CLASSIFICATION_IS_DELETED = "class_IsDeleted";

        public const int NAME_MAX_LENGTH = 128;
        public const int IMAGE_MAX_LENGTH = 128;
        public const int ICON_MAX_LENGTH = 128;
        public const int STYLE_MAX_LENGTH = 128;

        public const string FOREIGN_KEY_EXISTS = "{0} cannot be deleted because it is being referenced by another entity.";
        public const string ERROR_NOT_EXISTS = "{0} was not found find by key.";
        public const string ERROR_EXISTS = "{0} already exists";

        public static string NotFound(List<int> ids) => $"Classifications with IDs = {string.Join(", ", ids)} were not found.";
    }
}
