namespace Commerce.Query.Application.DTOs.Shop
{
    public class RevenueDTO
    {
        public int TotalOrder { get; set; }
        public int TotalPending { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalCancel { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalRevenueProfit { get; set; }
        public int TotalProduct { get; set; }
        public List<ShopRevenueDTO> RevenueByShop { get; set; } = new();
    }

    public class ShopRevenueDTO
    {
        public Guid Id { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalRevenueProfit { get; set; }
    }
}