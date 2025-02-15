namespace Commerce.Command.Presentation.DTOs.User
{
    public class UpdateUserRequestDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? WardId { get; set; }
        public bool? IsDeleted { get; set; }
        public List<Guid?>? RoleId { get; set; }
    }
}
