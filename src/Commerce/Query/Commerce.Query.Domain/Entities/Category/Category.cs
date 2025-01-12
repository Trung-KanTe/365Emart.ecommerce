using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Category
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary>
    public class Category : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of category
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Image of category
        /// </summary>
        public string? Image {  get; set; }

        /// <summary>
        /// Style of category
        /// </summary>
        public string? Style { get; set; }

        /// <summary>
        /// User Id of category
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Views of category
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

        /// <summary>
        /// List ClassificationCategory of Category
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<ClassificationCategory>? ClassificationCategories { get; set; }

        public override void Validate()
        {
        }
    }
}