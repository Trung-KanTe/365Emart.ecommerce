using Commerce.Command.Contract.Abstractions;
using Commerce.Command.Contract.DependencyInjection.Options;
using Commerce.Command.Contract.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VNPAY.NET;

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
                    ValidateIssuer = true,                   
                    ValidateAudience = true,                
                    ValidateLifetime = true,                
                    ValidateIssuerSigningKey = true,        
                    ValidIssuer = jwtSettings.Issuer,       
                    ValidAudience = jwtSettings.Audience,   
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });
            services.AddScoped<IVnpay, Vnpay>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }      
    }
}
