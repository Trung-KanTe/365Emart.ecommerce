using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Product;

namespace Commerce.Query.Application.UserCases.Product
{
    /// <summary>
    /// Request to get all product
    /// </summary>
    public class GetAllProductQuery : IRequest<Result<List<Entities.Product>>>
    {
    }

    /// <summary>
    /// Handler for get all product request
    /// </summary>
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Result<List<Entities.Product>>>
    {
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Handler for get all product request
        /// </summary>
        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list product as data</returns>
        public async Task<Result<List<Entities.Product>>> Handle(GetAllProductQuery request,
                                                       CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll().ToList();
            return await Task.FromResult(products);
        }
    }
}
