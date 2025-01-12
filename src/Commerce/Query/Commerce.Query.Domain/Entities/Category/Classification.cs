using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Category
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Classification : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of classification
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Icon of classification
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Style of classification
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// Views of classification
        /// </summary>
        public int? Views {  get; set; }

        /// <summary>
        /// Date when the User was created
        /// </summary>
        public DateTime? InsertedAt { get; set; } 

        /// <summary>
        /// User ID of the person who created this User
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// Date when the User was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; } 

        /// <summary>
        /// User ID of the person who last updated this User
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Status indicating if the User is deleted (soft delete)
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        public override void Validate()
        {
        }
    }
}