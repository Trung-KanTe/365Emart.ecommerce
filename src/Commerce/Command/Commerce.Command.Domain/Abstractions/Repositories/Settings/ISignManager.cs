using Entity = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Domain.Abstractions.Repositories.Settings
{
    public interface ISignManager
    {
        public Entity.User CurrentUser { get; }
    }
}
