namespace Commerce.Query.Contract.Shared
{
    public class LocalizationDetailDTO
    {
        public WebLocalizationWard Ward { get; set; }
        public WebLocalizationDistrict District { get; set; }
        public WebLocalizationProvince Province { get; set; }
        public WebLocalization Localization { get; set; }
    }
}