using Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.DTOs
{
    public class ImportProductDTO
    {
        public Guid? Id { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? WareHouseId { get; set; }
        public List<ImportProductDetails>? ImportProductDetails { get; set; }
    }
}
