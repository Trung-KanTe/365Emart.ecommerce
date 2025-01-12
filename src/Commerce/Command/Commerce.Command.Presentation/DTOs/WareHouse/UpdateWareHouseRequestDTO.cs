namespace Commerce.Command.Presentation.DTOs.WareHouse
{
    public class UpdateWareHouseRequestDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
