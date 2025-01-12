using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Shop;

namespace Commerce.Query.Application.UserCases.Shop
{
    /// <summary>
    /// Request to get shop by id
    /// </summary>
    public record GetShopByIdQuery : IRequest<Result<Entities.Shop>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get shop by id request
    /// </summary>
    public class GetShopByIdQueryHandler : IRequestHandler<GetShopByIdQuery, Result<Entities.Shop>>
    {
        private readonly IShopRepository shopRepository;

        /// <summary>
        /// Handler for get shop by id request
        /// </summary>
        public GetShopByIdQueryHandler(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with shop data</returns>
        public async Task<Result<Entities.Shop>> Handle(GetShopByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find shop without allow null return. If shop not found will throw NotFoundException
            var shop = await shopRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return shop!;
        }
    }
}
