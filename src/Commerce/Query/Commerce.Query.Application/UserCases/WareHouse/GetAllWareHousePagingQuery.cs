using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using MediatR;
using System.Linq.Expressions;
using Entities = Commerce.Query.Domain.Entities.WareHouse;

namespace Commerce.Query.Application.UserCases.WareHouse
{
    /// <summary>
    /// Request to get all wareHouse
    /// </summary>
    public class GetAllWareHousePagingQuery : IRequest<Result<PaginatedResult<Entities.WareHouse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        public GetAllWareHousePagingQuery(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }

    /// <summary>
    /// Handler for get all wareHouse request
    /// </summary>
    public class GetAllWareHousePagingQueryHandler : IRequestHandler<GetAllWareHousePagingQuery, Result<PaginatedResult<Entities.WareHouse>>>
    {
        private readonly IWareHouseRepository wareHouseRepository;

        public GetAllWareHousePagingQueryHandler(IWareHouseRepository wareHouseRepository)
        {
            this.wareHouseRepository = wareHouseRepository;
        }

        public async Task<Result<PaginatedResult<Entities.WareHouse>>> Handle(GetAllWareHousePagingQuery request, CancellationToken cancellationToken)
        {
            int pageNumber = request.PageNumber;
            int pageSize = 5;

            // Định nghĩa predicate (có thể áp dụng thêm điều kiện tìm kiếm nếu cần)
            Expression<Func<Entities.WareHouse, bool>> predicate = wareHouse => true;

            // Gọi phương thức GetPaginatedResultAsync từ repository để lấy dữ liệu phân trang
            var paginatedWareHouses = await wareHouseRepository.GetPaginatedResultAsync(
                pageNumber,
                pageSize,
                predicate,
                isTracking: false,
                includeProperties: Array.Empty<Expression<Func<Entities.WareHouse, object>>>()
            );

            // Trả về kết quả dưới dạng Result<PaginatedResult<Entities.WareHouse>>
            var result = new PaginatedResult<Entities.WareHouse>(
                pageNumber,
                pageSize,
                paginatedWareHouses.TotalCount,
                paginatedWareHouses.Items
            );

            return await Task.FromResult(result);
        }
    }
}
