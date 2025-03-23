namespace Commerce.Command.Presentation.DTOs.User
{
    public class UpdateUserPasswordDTO
    {
        public string? OldPasswordHash { get; set; }
        public string? NewPasswordHash { get; set; }
    }
}