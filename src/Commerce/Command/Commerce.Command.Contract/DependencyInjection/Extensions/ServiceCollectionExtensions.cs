using Commerce.Command.Contract.DependencyInjection.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Commerce.Command.Contract.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);

            // Register JwtSettings for DI
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Cấu hình Authentication và JWT Bearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                   // Kiểm tra Issuer
                    ValidateAudience = true,                // Kiểm tra Audience
                    ValidateLifetime = true,                // Kiểm tra thời gian hết hạn
                    ValidateIssuerSigningKey = true,        // Kiểm tra khóa ký
                    ValidIssuer = jwtSettings.Issuer,       // Giá trị Issuer từ cấu hình
                    ValidAudience = jwtSettings.Audience,   // Giá trị Audience từ cấu hình
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            return services;
        }      
    }
}
