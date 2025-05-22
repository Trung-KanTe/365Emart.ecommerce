using Commerce.Command.Contract.Abstractions;
using Entities = Commerce.Command.Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Entity = Commerce.Command.Domain.Entities.Shop;
using System.Text;

namespace Commerce.Command.Persistence.Repositories
{
    public class LowStockNotifier : ILowStockNotifier
    {
        protected readonly ApplicationDbContext context;
        private readonly IEmailSender emailSender;
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.ProductDetail> ProductDetails { get; set; }
        public DbSet<Entity.Shop> Shops { get; set; }

        public LowStockNotifier(ApplicationDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            this.emailSender = emailSender;
        }

        protected DbSet<Entities.Product> Product
        {
            get
            {
                if (Products == null) Products = context.Set<Entities.Product>();
                return Products;
            }
        }

        protected DbSet<Entities.ProductDetail> ProductDetail
        {
            get
            {
                if (ProductDetails == null) ProductDetails = context.Set<Entities.ProductDetail>();
                return ProductDetails;
            }
        }

        protected DbSet<Entity.Shop> Shop
        {
            get
            {
                if (Shops == null) Shops = context.Set<Entity.Shop>();
                return Shops;
            }
        }

        public async Task NotifyShopsWithLowStockAsync()
        {
            var lowStockProducts = await context.ProductDetails
                .Where(pd => pd.StockQuantity < 10)
                .Include(pd => pd.Product)
                .ThenInclude(p => p.Shop)
                .ToListAsync();

            var shopGroups = lowStockProducts
                .GroupBy(pd => pd.Product.ShopId)
                .ToList();

            foreach (var group in shopGroups)
            {
                var shop = await context.Shops
                    .FirstOrDefaultAsync(s => s.Id == group.Key);

                if (shop == null || string.IsNullOrEmpty(shop.Email))
                    continue;

                var sb = new StringBuilder();
                sb.Append("<h3>Các sản phẩm tồn kho thấp dưới 10</h3><ul>");

                foreach (var item in group)
                {
                    sb.Append($"<li>{item.Product.Name} - Atribute: {item.Size}, Số lượng: {item.StockQuantity}</li>");
                }

                sb.Append("</ul>");

                await emailSender.SendEmailAsync(shop.Email, "Thông báo tồn kho thấp", sb.ToString());
            }
        }
    }
}