using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Category
{
    /// <summary>
    /// Domain entity with int key type
    /// </summary
    public class ClassificationCategory : AggregateRoot<Guid>
    {
        /// <summary>
        /// Category Id of ClassificationCategory
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Classification Id of ClassificationCategory
        /// </summary>
        public Guid? ClassificationId { get; set; }

        /// <summary>
        /// Category of Classification
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public Category? Category { get; set; }

        public override void Validate()
        {
        }
    }
}