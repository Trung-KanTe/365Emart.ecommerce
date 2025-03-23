using Asp.Versioning;
using Commerce.Command.API.DependencyInjection.Extensions;
using Commerce.Command.API.DependencyInjection.Options;
using Commerce.Command.API.Middleware;
using Commerce.Command.Application.DependencyInjection.Extension;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Infrastructure.DependencyInjection.Extensions;
using Commerce.Command.Persistence.DependencyInjection.Extensions;
using Commerce.Command.Presentation.Abstractions;
using Commerce.Command.Presentation.Common;
using Hangfire;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
var serviceName = "E-Commerce";
//register controllers
builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
//register api configuration
builder.Services.AddSingleton(new ApiConfig { Name = serviceName });
//Configure swagger
builder.Services.ConfigureOptions<SwaggerConfigureOptions>();

//Configure api versioning
builder.Services.AddApiVersioning(
        options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader());
        })
    .AddMvc()
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin() 
               .AllowAnyMethod()  
               .AllowAnyHeader()); 
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseApiLayerSwagger();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapPresentation();
app.UseHangfireDashboard();

app.Run();