using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Ward
{
    public class Ward : AggregateRoot<int>
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public int DistrictId { get; set; }
        public override void Validate()
        {
        }
    }
}
