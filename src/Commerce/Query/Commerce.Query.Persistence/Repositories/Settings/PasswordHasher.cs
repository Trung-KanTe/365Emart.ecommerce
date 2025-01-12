using Microsoft.AspNetCore.Identity;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Persistence.Repositories.Settings
{
    public class PasswordHasher : IPasswordHasher<Entities.User>
    {
        private readonly PasswordHasher<Entities.User> hasher = new();

        public string HashPassword(Entities.User user, string password)
        {
            return hasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(Entities.User user, string hashedPassword, string providedPassword)
        {
            return hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
