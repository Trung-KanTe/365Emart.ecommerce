namespace Commerce.Query.Application.DTOs.Shop
{
    public class ProductSellerDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal? TotalAmount { get; set; }
        public int OrderCount { get; set; } 

        public decimal OrderPercentage(int totalOrders) =>
            totalOrders > 0 ? Math.Round((decimal)OrderCount / totalOrders * 100, 2) : 0;

        public decimal RevenuePercentage(int totalRevenue) =>
            totalRevenue > 0 ? Math.Round((decimal)(TotalAmount ?? 0) / totalRevenue * 100, 2) : 0;
    }
}