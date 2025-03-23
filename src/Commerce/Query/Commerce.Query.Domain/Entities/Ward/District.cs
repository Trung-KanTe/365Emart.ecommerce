using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Ward
{
    public class District : AggregateRoot<int>
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public int ProvinceId { get; set; }
        public override void Validate()
        {
        }
    }
}