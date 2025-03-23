namespace Commerce.Query.Domain.Constants.Ward
{
    public class WardConst
    {
        public const string TABLE_USER = "Ward";
        public const string FIELD_USER_ID = "war_Id";
        public const string FIELD_USER_NAME = "war_Name";
        public const string FIELD_USER_FULL_NAME = "war_FullName";
        public const string FIELD_USER_DISTRICT = "DistrictId";

        public const string TABLE_DISTRICT = "Districts";
        public const string FIELD_DISTRICT_ID = "district_id";
        public const string FIELD_DISTRICT_NAME = "name";
        public const string FIELD_DISTRICT_FULL_NAME = "full_name";
        
        public const string TABLE_PROVINCE = "Provinces";
        public const string FIELD_PROVINCE_ID = "province_id";
        public const string FIELD_PROVINCE_NAME = "name";
        public const string FIELD_PROVINCE_FULL_NAME = "full_name";
        public const string FIELD_PROVINCE_KEY = "key_localization";
    }
}
