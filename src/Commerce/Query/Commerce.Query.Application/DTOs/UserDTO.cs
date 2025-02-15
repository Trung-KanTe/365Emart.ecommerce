using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Application.DTOs
{
    public class UserDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Tel { get; set; }
        public string? Address { get; set; }
        public int? WardId { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public LocalizationFullDTO? LocalizationFullDTO { get; set; }
        public ICollection<Role>? Roles { get; set; }
    }
}