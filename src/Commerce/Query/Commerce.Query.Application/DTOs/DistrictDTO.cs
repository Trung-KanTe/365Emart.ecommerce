namespace Commerce.Query.Application.DTOs
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public int ProvinceId { get; set; }
        public ProvinceDTO province { get; set; }
    }
}