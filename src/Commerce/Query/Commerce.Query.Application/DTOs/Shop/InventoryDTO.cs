using Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.DTOs.Shop
{
    public class InventoryDTO
    {
        public Guid Id { get; set; }  
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ShopName { get; set; }
        public int Views { get; set; } = 0; 
        public decimal ImportPrice { get; set; } = 0;  
        public decimal SellingPrice { get; set; } = 0;
        public string? Image { get; set; }
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public List<ProductDetail>? ProductDetails { get; set; } = new();
        public int TotalStock => ProductDetails?.Sum(pd => pd.StockQuantity) ?? 0;
    }
}