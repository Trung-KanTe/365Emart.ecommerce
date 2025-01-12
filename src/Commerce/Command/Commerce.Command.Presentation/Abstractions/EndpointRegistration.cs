using Asp.Versioning;
using Commerce.Command.Presentation.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Commerce.Command.Presentation.Abstractions
{
    public static class EndpointRegistration
    {
        /// <summary>
        /// Map presentation layer endpoints
        /// </summary>
        /// <param name="app"></param>
        /// <returns>The endpoint route builder with mapped presentation layer endpoints</returns>
        public static IEndpointRouteBuilder MapPresentation(this IEndpointRouteBuilder app)
        {
            app.MapControllers();           
            return app;
        }
    }
}