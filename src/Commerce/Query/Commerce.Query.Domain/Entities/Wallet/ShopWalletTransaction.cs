using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Wallets
{
    public class ShopWalletTransaction : AggregateRoot<Guid>
    {
        public Guid ShopWalletId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public override void Validate()
        {
        }
    }
}
