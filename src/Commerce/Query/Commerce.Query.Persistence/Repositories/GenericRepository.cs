using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Entities;
using Commerce.Query.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace Commerce.Query.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IGenericRepository
    /// </summary>
    /// <typeparam name="TEntity">Generic type of Domain entity</typeparam>
    /// <typeparam name="TKey">Generic key of Domain entity</typeparam>
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        protected readonly ApplicationDbContext context;
        protected DbSet<TEntity>? entities;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get entity DbSet
        /// </summary>
        protected DbSet<TEntity> Entities
        {
            get
            {
                if (entities == null) entities = context.Set<TEntity>();
                return entities;
            }
        }

        /// <summary>
        /// Find entity by id. Returned entity can be tracking
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="option">Option to find entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<TEntity?> FindByIdAsync(TKey id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            var query = Entities.AsQueryable();
            if (includeProperties.Any())
                // Include specified properties
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();
            // Find entity by Id
            var result = await query.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
            return result;
        }

        /// <summary>
        /// Find single entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="option"></param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity matched expression or null if entity not found</returns>
        public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate,
                                                    bool isTracking = false,
                                                    CancellationToken cancellationToken = default,
                                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            var query = Entities.AsQueryable();
            if (includeProperties.Any())
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();
            // Apply predicate if provided, otherwise return a single entity
            var result = predicate is not null ? await query.FirstOrDefaultAsync(predicate, cancellationToken) : await query.FirstOrDefaultAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Check entity with specific predicate is exist in current application
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if entity exist, otherwise false</returns>
        public Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = Entities.AsQueryable();
            var result = query.Where(predicate);
            if (result.Any())
                return Task.FromResult(true);
            return Task.FromResult(false);
        }

        /// <summary>
        /// Find all entity that satisfied predicate expression. Can be tracking
        /// </summary>
        /// <param name="isTracking">Tracking state of entity</param>
        /// <param name="predicate">Predicate expression</param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>IQueryable of entities that match predicate expression</returns>
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, bool isTracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Initialize query from the entity set
            var query = Entities.AsQueryable();
            if (includeProperties.Any())
                query = IncludeMultiple(query, includeProperties);

            // Apply tracking option
            query = isTracking ? query : query.AsNoTracking();
            // Apply predicate if provided, otherwise return the query
            return predicate is not null ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Marked entity as Added state
        /// </summary>
        /// <param name="entity">Added entity</param>
        public void Create(TEntity entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Marked entity as Added state
        /// </summary>
        /// <param name="entity">Added entity</param>
        public void CreateMultiple(List<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        /// <summary>
        /// Marked entity as Updated state
        /// </summary>
        /// <param name="entity">Updated entity</param>
        public void Update(TEntity entity)
        {
            Entities.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Marked entity as Deleted state
        /// </summary>
        /// <param name="entity">Removed entity</param>
        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// Marked multiple entities as Deleted state
        /// </summary>
        /// <param name="entitiesToRemove">Removed entities</param>
        public void RemoveMultiple(List<TEntity> entitiesToRemove)
        {
            Entities.RemoveRange(entitiesToRemove);
        }

        /// <summary>
        /// Apply all changes in context to database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of changes are made to database</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            return transaction.GetDbTransaction();
        }

        /// <summary>
        /// Extension method of IQueryable for including multiple relationship
        /// </summary>
        /// <typeparam name="TEntity">Type of Domain entity</typeparam>
        /// <param name="source">IQueryable source need to including properties</param>
        /// <param name="includeProperties">Properties to be included</param>
        /// <returns>IQueryable with included properties</returns>
        private IQueryable<TEntity> IncludeMultiple(IQueryable<TEntity> source, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties.Any())
                // Each property will be included into source
                source = includeProperties.Aggregate(source, (current, include) => current.Include(include));
            return source;
        }

        public async Task<PaginatedResult<TEntity>> GetPaginatedResultAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            bool isTracking = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            // Tính tổng số bản ghi
            var query = FindAll(predicate, isTracking, includeProperties);
            var totalCount = await query.CountAsync();

            // Lấy dữ liệu phân trang
            var items = await query
                .Skip((pageNumber - 1) * pageSize) // Bỏ qua số bản ghi ở các trang trước
                .Take(pageSize) // Lấy số bản ghi theo kích thước trang
                .ToArrayAsync();

            // Tạo đối tượng PaginatedResult để trả về
            var result = new PaginatedResult<TEntity>(
                pageNumber,
                pageSize,
                totalCount,
                items
            );

            return result;
        }
    }
}