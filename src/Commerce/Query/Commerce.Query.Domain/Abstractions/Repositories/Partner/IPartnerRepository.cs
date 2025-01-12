using Entity = Commerce.Query.Domain.Entities.Partner;

namespace Commerce.Query.Domain.Abstractions.Repositories.Partner
{
    /// <summary>
    /// Provide commerce repository
    /// </summary>
    public interface IPartnerRepository : IGenericRepository<Entity.Partner, Guid>
    {
    }
}