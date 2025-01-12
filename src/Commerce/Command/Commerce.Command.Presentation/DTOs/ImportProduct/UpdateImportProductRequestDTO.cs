using Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Presentation.DTOs.ImportProduct
{
    public class UpdateImportProductRequestDTO
    {
        public Guid? PartnerId { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? WareHouseId { get; set; }
        public DateTime? ImportDate { get; set; }
        public string? Note { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<ImportProductDetails>? ImportProductDetails { get; set; }
    }
}
