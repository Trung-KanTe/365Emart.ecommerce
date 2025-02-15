using Commerce.Query.Contract.Constants;
using Commerce.Query.Contract.DependencyInjection.Options;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Commerce.Query.Contract.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Using Masstransit rabbitMQ as message broker
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="consumersAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration, Assembly? consumersAssembly = null)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            configuration.GetSection(Const.BROKER_CONFIG).Bind(rabbitMqOptions);
            services.AddMassTransit(busConfigurator =>
            {
                if (consumersAssembly is not null) busConfigurator.AddConsumers(consumersAssembly);
                //ex: get-sample-by-id-request
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                //Using rabbitMq for message broker
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(rabbitMqOptions.Host), host =>
                    {
                        host.Username(rabbitMqOptions.Username);
                        host.Password(rabbitMqOptions.Password);
                        host.RequestedConnectionTimeout(TimeSpan.FromSeconds(10));
                    });
                    configurator.ConfigureEndpoints(context);
                    configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(15)));
                    //  configurator.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)));
                });
            });
            return services;
        }

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
