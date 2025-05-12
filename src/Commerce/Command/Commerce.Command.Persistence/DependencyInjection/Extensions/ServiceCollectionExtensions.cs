using Commerce.Command.Domain.Abstractions.Repositories;
using Commerce.Command.Domain.Abstractions.Repositories.Brand;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Command.Domain.Abstractions.Repositories.Order;
using Commerce.Command.Domain.Abstractions.Repositories.Partner;
using Commerce.Command.Domain.Abstractions.Repositories.Payment;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.Settings;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using Commerce.Command.Domain.Abstractions.Repositories.User;
using Commerce.Command.Domain.Abstractions.Repositories.Wallets;
using Commerce.Command.Domain.Abstractions.Repositories.WareHouse;
using Commerce.Command.Domain.Entities.User;
using Commerce.Command.Persistence.DependencyInjection.Options;
using Commerce.Command.Persistence.Repositories;
using Commerce.Command.Persistence.Repositories.Brand;
using Commerce.Command.Persistence.Repositories.Cart;
using Commerce.Command.Persistence.Repositories.Category;
using Commerce.Command.Persistence.Repositories.Classification;
using Commerce.Command.Persistence.Repositories.ImportProduct;
using Commerce.Command.Persistence.Repositories.Order;
using Commerce.Command.Persistence.Repositories.Partner;
using Commerce.Command.Persistence.Repositories.Payment;
using Commerce.Command.Persistence.Repositories.Product;
using Commerce.Command.Persistence.Repositories.Promotion;
using Commerce.Command.Persistence.Repositories.Settings;
using Commerce.Command.Persistence.Repositories.Shop;
using Commerce.Command.Persistence.Repositories.User;
using Commerce.Command.Persistence.Repositories.WareHouse;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Command.Persistence.DependencyInjection.Extensions
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
            services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            services.AddScoped<IProductStockRepository, ProductStockRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();
            services.AddScoped<IImportProductRepository, ImportProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<IProductReviewReplyRepository, ProductReviewReplyRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderCancelRepository, OrderCancelRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IShopWalletRepository, ShopWalletRepository>();
            services.AddScoped<IShopWalletTransactionRepository, ShopWalletTransactionRepository>();
            services.AddScoped<IPlatformWalletRepository, PlatformWalletRepository>();
            services.AddScoped<IPlatformWalletTransactionRepository, PlatformWalletTransactionRepository>();
            return services;
        }
    }
}