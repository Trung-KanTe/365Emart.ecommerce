using Commerce.Query.Application.DTOs.Shop;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get brand by id
    /// </summary>
    public record GetAllInventoryQuery : IRequest<Result<List<InventoryDTO>>>
    {

        public GetAllInventoryQuery()
        {
        }
    }

    /// Handle request
    /// </summary>
    /// <param name="request">Request to handle</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with brand data</returns>
    public class GetAllInventoryQueryHandler : IRequestHandler<GetAllInventoryQuery, Result<List<InventoryDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;
        private readonly IShopRepository shopRepository;
        private readonly IImportProductDetailRepository importProductDetailRepository;

        public GetAllInventoryQueryHandler(
            IProductRepository productRepository,
            IProductDetailRepository productDetailRepository,
            IShopRepository shopRepository,
            IImportProductDetailRepository importProductDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
            this.shopRepository = shopRepository;
            this.importProductDetailRepository = importProductDetailRepository;
        }

        public async Task<Result<List<InventoryDTO>>> Handle(GetAllInventoryQuery request, CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll(x => x.ProductDetails != null && x.ProductDetails.Any(),
                                             true, x => x.ProductDetails!).ToList();

            var shops = shopRepository.FindAll().ToList();
            var shopDictionary = shops.ToDictionary(s => s.Id, s => s.Name ?? "Unknown");

            var inventoryList = new List<InventoryDTO>();

            foreach (var product in products)
            {
                var totalStock = product.ProductDetails!.Sum(pd => pd.StockQuantity);

                if (totalStock >= 20) continue; 

                var firstProductDetail = product.ProductDetails!.OrderBy(pd => pd.Id).First();

                var importDetail = await importProductDetailRepository
                    .FindSingleAsync(ipd => ipd.ProductDetailId == firstProductDetail.Id, true, cancellationToken);

                decimal importPrice = importDetail?.ImportPrice ?? 0;

                inventoryList.Add(new InventoryDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ShopName = shopDictionary.TryGetValue(product.ShopId!.Value, out var shopName) ? shopName : "Unknown",
                    Views = product.Views ?? 0,
                    ImportPrice = importPrice,
                    SellingPrice = product.Price ?? 0,
                    Image = product.Image,
                    InsertedAt = product.InsertedAt ?? DateTime.UtcNow,
                    IsDeleted = product.IsDeleted ?? false,
                    ProductDetails = product.ProductDetails!.ToList()
                });
            }

            return inventoryList;
        }
    }
}