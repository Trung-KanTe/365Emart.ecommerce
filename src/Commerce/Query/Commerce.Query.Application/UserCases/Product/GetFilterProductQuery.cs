using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Helpers;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all product
    /// </summary>
    public class GetFilterProductQuery : IRequest<Result<List<ProductDTO>>>
    {
        public SearchCommand? SearchCommand { get; set; }
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class GetFilterProductQueryHandler : IRequestHandler<GetFilterProductQuery, Result<List<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for get all product request
        /// </summary>
        public GetFilterProductQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
        {
            this.productRepository = productRepository;
            this.productDetailRepository = productDetailRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list product as data</returns>
        public async Task<Result<List<ProductDTO>>> Handle(GetFilterProductQuery request,
                                                       CancellationToken cancellationToken)
        {
            var productsQuery = productRepository.FindAll().ApplySearch(request.SearchCommand!);
            List<Entities.Product> products = productsQuery.ToList();
            List<ProductDTO> productDtos = products.Select(product =>
            {
                ProductDTO productDto = product.MapTo<ProductDTO>()!;
                productDto.ProductDetails = productDetailRepository.FindAll(x => x.ProductId == product.Id).ToList().Select(productItem => productItem.MapTo<Entities.ProductDetail>()!).ToList();
                return productDto;
            }).ToList();

            return productDtos;
        }
    }
}
