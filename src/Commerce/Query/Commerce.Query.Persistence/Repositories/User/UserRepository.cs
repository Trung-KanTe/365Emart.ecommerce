using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Persistence.Repositories.User
{
    /// <summary>
    /// Implementation of ISampleRepository
    /// </summary>
    public class UserRepository : GenericRepository<Entities.User, Guid>, IUserRepository
    {
        private readonly ISignManager signManager;

        /// <summary>
        /// Implementation of ISampleRepository
        /// </summary>
        public UserRepository(ApplicationDbContext context, ISignManager signManager) : base(context)
        {
            this.signManager = signManager;
        }

        public virtual void Create(Entities.User entity)
        {
            entity.InsertedAt = DateTime.UtcNow;
            entity.InsertedBy = signManager.CurrentUser?.Id ?? null;
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            Entities.Add(entity);
        }

        public virtual void Update(Entities.User entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = signManager.CurrentUser.Id;
            Entities.Update(entity);
        }
    }
}