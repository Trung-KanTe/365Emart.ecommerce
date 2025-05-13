using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.WareHouse;

namespace Commerce.Query.Domain.Entities.WareHouse
{
    public class WareHouse : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Warehouse
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Address of the Warehouse
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the Warehouse's location
        /// </summary>
        public Guid? WardId { get; set; }
        public Guid? ShopId { get; set; }

        /// <summary>
        /// Date when the Warehouse was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Warehouse
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Warehouse was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Warehouse
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the Warehouse is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        public override void Validate()
        {
        }
    }
}
