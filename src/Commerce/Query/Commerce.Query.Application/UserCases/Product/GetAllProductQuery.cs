using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all product
    /// </summary>
    public class GetAllProductQuery : IRequest<Result<List<ProductDTO>>>
    {
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Result<List<ProductDTO>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for get all product request
        /// </summary>
        public GetAllProductQueryHandler(IProductRepository productRepository, IProductDetailRepository productDetailRepository)
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
        public async Task<Result<List<ProductDTO>>> Handle(GetAllProductQuery request,
                                                       CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll().ToList();
            List<ProductDTO> orderDtos = products.Select(order =>
            {
                ProductDTO orderDto = order.MapTo<ProductDTO>()!;
                orderDto.ProductDetails = productDetailRepository.FindAll(x => x.ProductId == order.Id).ToList().Select(orderItem => orderItem.MapTo<Entities.ProductDetail>()!).ToList();
                return orderDto;
            }).ToList();

            return orderDtos;
        }
    }
}