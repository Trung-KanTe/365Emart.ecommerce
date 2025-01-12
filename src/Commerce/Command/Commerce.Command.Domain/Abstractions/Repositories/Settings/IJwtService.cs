using Entity = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Domain.Abstractions.Repositories.Settings
{
    public interface IJwtService
    {
        string GenerateToken(Entity.User user, IList<string> roles);
    }
}
