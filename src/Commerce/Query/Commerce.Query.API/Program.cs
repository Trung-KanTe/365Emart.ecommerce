using Asp.Versioning;
using Commerce.Query.API.DependencyInjection.Extensions;
using Commerce.Query.API.DependencyInjection.Options;
using Commerce.Query.API.Middleware;
using Commerce.Query.Application.DependencyInjection.Extension;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Persistence.DependencyInjection.Extensions;
using Commerce.Query.Presentation.Abstractions;
using Commerce.Query.Presentation.Common;
using Microsoft.Extensions.FileProviders;
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
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration);
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
var uploadFolderPath = Path.Combine(builder.Environment.ContentRootPath, "UploadFile", "Image");

// In ra ???ng d?n th?c t? c?a th? m?c ?nh
Console.WriteLine($"Serving static files from: {uploadFolderPath}");

if (!Directory.Exists(uploadFolderPath))
{
    Directory.CreateDirectory(uploadFolderPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadFolderPath),
    RequestPath = "/images",
    ServeUnknownFileTypes = true, 
});
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapPresentation();
app.Run();