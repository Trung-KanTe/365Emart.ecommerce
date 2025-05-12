namespace Commerce.Query.Application.DTOs.Shop
{
    public class WalletDTO
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public decimal Balance { get; set; }
        public List<WalletTransactionDTO>? WalletTransactionDTOs { get; set; }
    }
}