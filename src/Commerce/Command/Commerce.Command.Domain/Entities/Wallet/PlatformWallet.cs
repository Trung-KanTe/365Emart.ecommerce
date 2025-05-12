using Commerce.Command.Domain.Abstractions.Aggregates;

namespace Commerce.Command.Domain.Entities.Wallets
{
    public class PlatformWallet : AggregateRoot<Guid>
    {
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
