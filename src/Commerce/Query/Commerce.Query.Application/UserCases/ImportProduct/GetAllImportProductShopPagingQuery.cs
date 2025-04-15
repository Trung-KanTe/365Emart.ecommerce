using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to get all importProduct
    /// </summary>
    public class GetAllImportProductShopPagingQuery : IRequest<Result<PaginatedResult<ImportProductDTO>>>
    {
        public Guid? Id { get; init; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllImportProductShopPagingQuery(Guid? id, int pageNumber, int pageSize)
        {
            Id = id;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Handler for get all importProduct request
    /// </summary>
    public class GetAllImportProductShopPagingQueryHandler : IRequestHandler<GetAllImportProductShopPagingQuery, Result<PaginatedResult<ImportProductDTO>>>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IWareHouseRepository wareHouseRepository;
        private readonly IShopRepository shopRepository;

        public GetAllImportProductShopPagingQueryHandler(
            IImportProductRepository importProductRepository,
            IPartnerRepository partnerRepository,
            IWareHouseRepository wareHouseRepository,
            IShopRepository shopRepository)
        {
            this.importProductRepository = importProductRepository;
            this.partnerRepository = partnerRepository;
            this.wareHouseRepository = wareHouseRepository;
            this.shopRepository = shopRepository;
        }

        public async Task<Result<PaginatedResult<ImportProductDTO>>> Handle(GetAllImportProductShopPagingQuery request, CancellationToken cancellationToken)
        {
            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.Id, true, cancellationToken);

            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.ImportProduct, bool>> predicate = importProduct => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedImportProducts = await importProductRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate: product => product.ShopId == shop.Id,
                isTracking: false,
                includeProperties: p => p.ImportProductDetails!  // Include ImportProductDetails
            );

            // Ánh xạ các Entity ImportProduct sang ImportProductDTO
            var importProductDTOs = new List<ImportProductDTO>();

            foreach (var importProduct in paginatedImportProducts.Items)
            {
                // Truy vấn thông tin Partner, WareHouse, Shop cho từng sản phẩm
                var partner = await partnerRepository.FindByIdAsync(importProduct.PartnerId!.Value, true, cancellationToken);
                var wareHouse = await wareHouseRepository.FindByIdAsync(importProduct.WareHouseId!.Value, true, cancellationToken);

                // Ánh xạ vào ImportProductDTO
                var importProductDTO = new ImportProductDTO
                {
                    Id = importProduct.Id,
                    Note = importProduct.Note,
                    ImportDate = importProduct.ImportDate,
                    Partner = partner != null ? new PartnerDTO { Id = partner.Id, Name = partner.Name } : null,
                    WareHouse = wareHouse != null ? new WareHouseDTO { Id = wareHouse.Id, Name = wareHouse.Name } : null,
                    Shop = shop != null ? new ShopDTO { Id = shop.Id, Name = shop.Name } : null,
                    IsDeleted = importProduct.IsDeleted,
                    ImportProductDetails = importProduct.ImportProductDetails!.Select(pd => new Entities.ImportProductDetails
                    {
                        Id = pd.Id,
                        ProductDetailId = pd.ProductDetailId,
                        ImportPrice = pd.ImportPrice,
                        Quantity = pd.Quantity,
                        ImportProductId = pd.ImportProductId,
                    }).ToList()
                };

                importProductDTOs.Add(importProductDTO);
            }

            // Trả về kết quả dưới dạng Result<PaginatedResult<ImportProductDTO>>
            var result = new PaginatedResult<ImportProductDTO>(
                pageNumber,
                pageSize,
                paginatedImportProducts.TotalCount,
                importProductDTOs
            );

            return await Task.FromResult(result);
        }
    }
}
