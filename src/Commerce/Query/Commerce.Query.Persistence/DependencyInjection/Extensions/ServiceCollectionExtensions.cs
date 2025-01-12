using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Domain.Abstractions.Repositories;
using Commerce.Query.Domain.Abstractions.Repositories.Brand;
using Commerce.Query.Domain.Abstractions.Repositories.Cart;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using Commerce.Query.Domain.Abstractions.Repositories.Payment;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Promotion;
using Commerce.Query.Domain.Abstractions.Repositories.Settings;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using Commerce.Query.Domain.Abstractions.Repositories.User;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using Commerce.Query.Domain.Entities.User;
using Commerce.Query.Persistence.DependencyInjection.Options;
using Commerce.Query.Persistence.Repositories;
using Commerce.Query.Persistence.Repositories.Brand;
using Commerce.Query.Persistence.Repositories.Cart;
using Commerce.Query.Persistence.Repositories.Category;
using Commerce.Query.Persistence.Repositories.Classification;
using Commerce.Query.Persistence.Repositories.ImportProduct;
using Commerce.Query.Persistence.Repositories.Order;
using Commerce.Query.Persistence.Repositories.Partner;
using Commerce.Query.Persistence.Repositories.Payment;
using Commerce.Query.Persistence.Repositories.Product;
using Commerce.Query.Persistence.Repositories.Promotion;
using Commerce.Query.Persistence.Repositories.Settings;
using Commerce.Query.Persistence.Repositories.Shop;
using Commerce.Query.Persistence.Repositories.User;
using Commerce.Query.Persistence.Repositories.WareHouse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Query.Persistence.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register infrastructure EF services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            var connectionStringOptions = new ConnectionStringOptions();
            configuration.GetSection(ConnectionStringOptions.ConnectionStrings).Bind(connectionStringOptions);
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionStringOptions.SqlServer);
            });
            services.RegisterServices();
            //services.AddJWT(configuration);
            return services;
        }

        /// <summary>
        /// Registering infrastructure ef services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IClassificationRepository, ClassificationRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher>();
            services.AddScoped<ISignManager, SignManager>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();
            services.AddScoped<IImportProductRepository, ImportProductRepository>();
            services.AddScoped<IImportProductDetailRepository, ImportProductDetailRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderCancelRepository, OrderCancelRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
            return services;
        }
    }
}