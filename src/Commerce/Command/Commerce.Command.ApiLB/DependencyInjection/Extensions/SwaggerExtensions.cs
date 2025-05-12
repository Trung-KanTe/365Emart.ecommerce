using Asp.Versioning.ApiExplorer;

namespace Commerce.Command.API.DependencyInjection.Extensions
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// For registering swagger and swagger UI
        /// </summary>
        /// <param name="app"></param>
        /// <returns>The application builder with Swagger and Swagger UI registered</returns>
        public static IApplicationBuilder UseApiLayerSwagger(this IApplicationBuilder app)
        {
            // Serve Swagger JSON endpoint
            app.UseSwagger();
            // Serve Swagger UI
            app.UseSwaggerUI(options =>
            {
                // Get API version descriptions
                var versionDescriptor = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                // Add Swagger endpoints
                foreach (var descriptor in versionDescriptor.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{descriptor.GroupName}/swagger.json", 
                        $"{descriptor.GroupName}");
                }
            });
            return app;
        }
    }
}