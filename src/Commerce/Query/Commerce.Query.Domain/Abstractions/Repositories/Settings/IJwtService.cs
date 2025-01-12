using Entity = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Domain.Abstractions.Repositories.Settings
{
    public interface IJwtService
    {
        string GenerateToken(Entity.User user, IList<string> roles);
    }
}
