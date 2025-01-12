using Entity = Commerce.Command.Domain.Entities.Partner;

namespace Commerce.Command.Domain.Abstractions.Repositories.Partner
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPartnerRepository : IGenericRepository<Entity.Partner, Guid>
    {
    }
}