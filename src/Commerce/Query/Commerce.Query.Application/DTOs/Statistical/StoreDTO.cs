namespace Commerce.Query.Application.DTOs.Statistical
{
    public class StoreDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalGrossRevenue { get; set; }
        public decimal TotalNetRevenue { get; set; }
        public decimal TotalProfit { get; set; }
        public List<OrderDTO> OrderDTOs { get; set; }
    }
}