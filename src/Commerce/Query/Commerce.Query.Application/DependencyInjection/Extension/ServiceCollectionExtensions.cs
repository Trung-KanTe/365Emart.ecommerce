using Commerce.Query.Contract.Abstractions;
using Commerce.Query.Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Commerce.Query.Application.DependencyInjection.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register application services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register mediator
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()); });
            services.AddScoped<IWebWardService, WebWardServiceRabbit>();
            return services;
        }
    }
}