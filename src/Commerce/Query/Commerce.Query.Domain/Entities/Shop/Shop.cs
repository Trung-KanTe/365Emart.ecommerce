using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Brand;
using Commerce.Query.Domain.Constants.Shop;

namespace Commerce.Query.Domain.Entities.Shop
{
    public class Shop : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Shop
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Shop
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Image of the Shop
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Style of the Shop
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// Phone number of the Shop
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Email of the Shop
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Website of the Shop
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// Address of the Shop
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the Shop's location
        /// </summary>
        public Guid? WardId { get; set; }

        /// <summary>
        /// User ID associated with the Shop
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Number of views for the Shop
        /// </summary>
        public int? Views { get; set; } = 0;

        /// <summary>
        /// Date when the Shop was created
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// User ID of the person who created this Shop
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the Shop was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User ID of the person who last updated this Shop
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the Shop is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

        public override void Validate()
        {
        }
    }
}
