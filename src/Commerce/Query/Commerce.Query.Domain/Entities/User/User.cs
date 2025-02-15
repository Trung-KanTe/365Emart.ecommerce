using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.User
{
    public class User : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the User
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Email of the User
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Hashed Password of the User
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Phone number of the User
        /// </summary>
        public string? Tel { get; set; }

        /// <summary>
        /// Address of the User
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ward ID for the User's location
        /// </summary>
        public int? WardId { get; set; }

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
        public ICollection<UserRole>? UserRoles { get; set; }

        public override void Validate()
        {
        }
    }
}
