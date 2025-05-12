using Commerce.Command.Domain.Abstractions.Aggregates;

namespace Commerce.Command.Domain.Entities.Wallets
{
    public class PlatformWalletTransaction : AggregateRoot<Guid>
    {
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
