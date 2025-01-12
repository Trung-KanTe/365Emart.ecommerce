using Commerce.Query.Domain.Abstractions.Entities;

namespace Commerce.Query.Domain.Abstractions.Aggregates
{
    /// <summary>
    /// Aggregate root
    /// </summary>
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
    }
}