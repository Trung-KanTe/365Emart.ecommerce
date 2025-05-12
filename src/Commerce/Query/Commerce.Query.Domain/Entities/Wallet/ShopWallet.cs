using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Wallets
{
    public class ShopWallet : AggregateRoot<Guid>
    {
        public Guid ShopId { get; set; }
        public decimal Balance { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public override void Validate()
        {
        }
    }
}
