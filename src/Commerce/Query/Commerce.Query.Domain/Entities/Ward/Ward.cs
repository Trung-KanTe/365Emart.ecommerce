using Commerce.Query.Domain.Abstractions.Aggregates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.Ward
{
    public class Ward : AggregateRoot<int>
    {
        /// <summary>
        /// Name of the Ward
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Name of the Ward
        /// </summary>
        public string? FullName { get; set; }
        public override void Validate()
        {
        }
    }
}
