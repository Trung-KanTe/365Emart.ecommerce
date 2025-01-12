using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Entities = Commerce.Command.Domain.Entities.User;

namespace Commerce.Command.Persistence.Repositories.Settings
{
    public class SignManager : ISignManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SignManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Entities.User CurrentUser
        {
            get
            {
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
                    return null;

                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                    return null;

                Entities.User user = new Entities.User
                {
                    Id = parsedUserId,
                    Name = userName,
                    Email = email
                };
                return user;
            }
        }
    }
}
