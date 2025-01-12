using Commerce.Query.Presentation.Common;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Commerce.Query.API.DependencyInjection.Options
{
    /// <summary>
    /// Options for configure swagger
    /// </summary>
    public class SwaggerConfigureOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly ApiConfig apiConfig;
        private readonly IApiVersionDescriptionProvider provider;

        public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider, ApiConfig apiConfig)
        {
            this.provider = provider;
            this.apiConfig = apiConfig;
        }

        /// <summary>
        /// Configure swagger gen
        /// </summary>
        /// <param name="options">Options to configure</param>
        public void Configure(SwaggerGenOptions options)
        {
            // Iterate through API version descriptions
            foreach (var description in provider.ApiVersionDescriptions)
            {
                // Create OpenAPI info
                var apiInfo = new OpenApiInfo
                {
                    Title = apiConfig.Name,
                    Version = description.ApiVersion.ToString(),
                    Description = description.IsDeprecated ? "This version was deprecated." : null
                };
                // Add Swagger document
                options.SwaggerDoc(description.GroupName, apiInfo);
            }
        }

        /// <summary>
        /// Configure swagger gen
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }
    }
}