using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to get all importProduct
    /// </summary>
    public class GetAllImportProductQuery : IRequest<Result<List<ImportProductDTO>>>
    {
        public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all importProduct request
    /// </summary>
    public class GetAllImportProductQueryHandler : IRequestHandler<GetAllImportProductQuery, Result<List<ImportProductDTO>>>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IImportProductDetailRepository importProductDetailRepository;

        /// <summary>
        /// Handler for get all importProduct request
        /// </summary>
        public GetAllImportProductQueryHandler(IImportProductRepository importProductRepository, IImportProductDetailRepository importProductDetailRepository)
        {
            this.importProductRepository = importProductRepository;
            this.importProductDetailRepository = importProductDetailRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list importProduct as data</returns>
        public async Task<Result<List<ImportProductDTO>>> Handle(GetAllImportProductQuery request,
                                                       CancellationToken cancellationToken)
        {
            var productsQuery = importProductRepository.FindAll().ApplySearch(request.SearchCommand!);
            List<Entities.ImportProduct> products = productsQuery.ToList();
            List<ImportProductDTO> productDtos = products.Select(product =>
            {
                ImportProductDTO productDto = product.MapTo<ImportProductDTO>()!;
                productDto.ImportProductDetails = importProductDetailRepository.FindAll(x => x.ImportProductId == product.Id).ToList().Select(productItem => productItem.MapTo<Entities.ImportProductDetails>()!).ToList();
                return productDto;
            }).ToList();

            return productDtos;
        }
    }
}
