namespace Commerce.Command.Application.UserCases.DTOs
{
    public class ImportProductDetailDTO
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ImportProductId { get; set; }
        public decimal? ImportPrice { get; set; }
        public int? Quantity { get; set; }
        public List<ProductDetailDTO>? ProductDetails { get; set; }
    }
}
