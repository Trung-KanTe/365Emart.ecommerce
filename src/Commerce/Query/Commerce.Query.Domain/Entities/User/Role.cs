using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;
using Commerce.Query.Domain.Constants.User;

namespace Commerce.Query.Domain.Entities.User
{
    public class Role : AggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the Role
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description of the Role
        /// </summary>
        public string? Description { get; set; }

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
