using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Commerce.Query.Domain.Entities.User
{
    public class UserRole 
    {
        /// <summary>
        /// User ID associated with the role
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Role ID associated with the user
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// Category of Classification
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public User? User { get; set; }       
    }
}
