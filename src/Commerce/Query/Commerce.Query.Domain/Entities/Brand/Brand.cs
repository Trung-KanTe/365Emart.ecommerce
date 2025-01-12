using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.Brand;

namespace Commerce.Query.Domain.Entities.Brand
{
    /// <summary>
    /// Domain entity with guid key type
    /// </summary>
    public class Brand : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of Brand
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Icon of Brand
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Style of Brand
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// User Id of Brand
        /// </summary>
        public Guid? UserId { get; set; }


        /// <summary>
        /// Views of Brand
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// Inserted date of the Brand
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// Inserted User of the Brand
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Update date of the Brand
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Updated User of the Brand
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Deleted status of the Brand
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        public override void Validate()
        {         
        }
    }
}