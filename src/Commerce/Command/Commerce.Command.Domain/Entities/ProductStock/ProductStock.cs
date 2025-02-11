using Commerce.Command.Domain.Abstractions.Aggregates;

namespace Commerce.Command.Domain.Entities.ProductStock
{
    /// <summary>
    /// Domain entity with guid key type
    /// </summary>
    public class ProductStock : AggregateRoot<Guid>
    {
        /// <summary>
        /// User Id of ProducStock
        /// </summary>
        public Guid? ProductId { get; set; }
        public Guid? WareHouseId { get; set; }
        public int? Quantity { get; set; }

        public override void Validate()
        {
            ValidateCreate();
            ValidateUpdate();
        }

        /// <summary>
        /// Validate when creating a new ProducStock
        /// </summary>
        public void ValidateCreate()
        {
            
        }

        /// <summary>
        /// Validate when updating an existing ProducStock
        /// </summary>
        public void ValidateUpdate()
        {
            
        }
    }
}