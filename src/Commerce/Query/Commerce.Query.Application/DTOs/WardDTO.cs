using Commerce.Query.Contract.Shared;

namespace Commerce.Query.Application.DTOs
{
    public class WardDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public LocalizationFullDTO? LocalizationFullDTO { get; set; }
    }
}