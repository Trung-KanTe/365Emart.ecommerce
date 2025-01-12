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

        public override void Validate()
        {
        }
    }
}
