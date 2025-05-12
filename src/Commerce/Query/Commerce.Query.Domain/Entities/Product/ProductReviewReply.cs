using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Aggregates;


namespace Commerce.Command.Domain.Entities.Product
{
    /// <summary>
    /// Represents a reply from a shop to a product review.
    /// </summary>
    public class ProductReviewReply : AggregateRoot<Guid>
    {
        /// <summary>
        /// The ID of the product review this reply is associated with.
        /// </summary>
        public Guid ReviewId { get; set; }

        /// <summary>
        /// The ID of the shop replying to the review.
        /// </summary>
        public Guid? ShopId { get; set; }

        /// <summary>
        /// The content of the reply from the shop.
        /// </summary>
        public string? Reply { get; set; }

        /// <summary>
        /// The date and time when the reply was created.
        /// </summary>
        public DateTime? InsertedAt { get; set; }

        /// <summary>
        /// The ID of the user who created the reply.
        /// </summary>
        public Guid? InsertedBy { get; set; }

        /// <summary>
        /// The date and time when the reply was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The ID of the user who last updated the reply.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Indicates whether the reply has been soft-deleted.
        /// </summary>
        public bool? IsDeleted { get; set; } = true;

        /// <summary>
        /// Validate the entity.
        /// </summary>
        public override void Validate()
        {
            ValidateCreate();
            ValidateUpdate();
        }

        /// <summary>
        /// Validate when creating a new reply.
        /// </summary>
        public void ValidateCreate()
        {
            var validator = Validator.Create(this);
            validator.Validate();
        }

        /// <summary>
        /// Validate when updating an existing reply.
        /// </summary>
        public void ValidateUpdate()
        {
            var validator = Validator.Create(this);
            validator.Validate();
        }
    }
}
