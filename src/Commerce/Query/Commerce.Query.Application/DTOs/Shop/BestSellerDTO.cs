namespace Commerce.Query.Application.DTOs.Shop
{
    public class BestSellerDTO
    {
        public int TotalOrder { get; set; }
        public int TotalRevenue { get; set; }
        public List<ProductSellerDTO> Products { get; set; }
    }
}