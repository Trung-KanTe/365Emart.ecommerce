namespace Commerce.Query.Application.DTOs.Shop
{
    public class WalletTransactionDTO
    {
        public Guid? Id { get; set; }
        public Guid ShopWalletId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}