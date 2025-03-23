namespace Commerce.Query.Application.DTOs
{
    public class WardFullDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public int DistrictId { get; set; }
        public DistrictDTO district { get; set; }
    }
}