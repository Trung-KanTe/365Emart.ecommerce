namespace Commerce.Command.Presentation.DTOs.User
{
    public class UpdateRoleRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
