using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Query.Domain.Abstractions.Repositories.Partner;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to get all importProduct
    /// </summary>
    public class GetAllImportProductPagingQuery : IRequest<Result<PaginatedResult<ImportProductDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllImportProductPagingQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all importProduct request
    /// </summary>
    public class GetAllImportProductPagingQueryHandler : IRequestHandler<GetAllImportProductPagingQuery, Result<PaginatedResult<ImportProductDTO>>>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IWareHouseRepository wareHouseRepository;

        public GetAllImportProductPagingQueryHandler(
            IImportProductRepository importProductRepository,
            IPartnerRepository partnerRepository,
            IWareHouseRepository wareHouseRepository)
        {
            this.importProductRepository = importProductRepository;
            this.partnerRepository = partnerRepository;
            this.wareHouseRepository = wareHouseRepository;
        }

        public async Task<Result<PaginatedResult<ImportProductDTO>>> Handle(GetAllImportProductPagingQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.ImportProduct, bool>> predicate = importProduct => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedImportProducts = await importProductRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
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
                    IsDeleted = importProduct.IsDeleted,
                    ImportProductDetails = importProduct.ImportProductDetails!.Select(pd => new Entities.ImportProductDetails
                    {
                        Id = pd.Id,
                        ProductId = pd.ProductId,
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
