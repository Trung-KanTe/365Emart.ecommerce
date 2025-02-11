namespace Commerce.Command.Application.UserCases.DTOs
{
    public class ImportProductDTO
    {
        public Guid? Id { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? WareHouseId { get; set; }
        public string? Note { get; set; }
        public bool? IsDeleted { get; set; } = true;
        public List<ImportProductDetailDTO>? ImportProductDetails { get; set; }
    }
}
