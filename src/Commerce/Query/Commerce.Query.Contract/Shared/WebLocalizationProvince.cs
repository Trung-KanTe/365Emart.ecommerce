namespace Commerce.Query.Contract.Shared
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class WebLocalizationProvince
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string KeyLocalization { get; set; }
    }
}