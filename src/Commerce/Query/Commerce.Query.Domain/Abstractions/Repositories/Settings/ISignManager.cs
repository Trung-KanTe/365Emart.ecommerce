using Entity = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Domain.Abstractions.Repositories.Settings
{
    public interface ISignManager
    {
        public Entity.User CurrentUser { get; }
    }
}