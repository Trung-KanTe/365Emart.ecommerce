namespace Commerce.Query.Contract.Shared
{
    public class LocalizationFullDTO
    {
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string LocalizationName { get; set; }
        public string NameDescending => $"{LocalizationName}, {ProvinceName}, {DistrictName}, {WardName}";
        public string NameAscending => $"{WardName}, {DistrictName}, {ProvinceName}, {LocalizationName}";
    }
}