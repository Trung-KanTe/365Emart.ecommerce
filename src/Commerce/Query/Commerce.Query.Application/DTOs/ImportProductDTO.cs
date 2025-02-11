using Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.DTOs
{
    public class ImportProductDTO
    {
        public Guid? Id { get; set; }
        public PartnerDTO? Partner { get; set; }
        public WareHouseDTO? WareHouse { get; set; }
        public string? Note { get; set; }
        public DateTime? ImportDate { get; set; }
        public bool? IsDeleted { get; set; }
        public List<ImportProductDetails>? ImportProductDetails { get; set; }
    }
}
