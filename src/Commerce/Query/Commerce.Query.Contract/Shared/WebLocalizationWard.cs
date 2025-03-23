namespace Commerce.Query.Contract.Shared
{
    public class WebLocalizationWard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int DistrictId { get; set; }
    }
}