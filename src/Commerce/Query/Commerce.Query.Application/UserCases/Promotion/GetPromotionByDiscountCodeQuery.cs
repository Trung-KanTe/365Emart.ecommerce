using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Promotion;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to get promotion by id
    /// </summary>
    public record GetPromotionByDiscountCodeQuery : IRequest<Result<Entities.Promotion>>
    {
        public string? DiscountCode { get; init; }
    }

    /// <summary>
    /// Handler for get promotion by id request
    /// </summary>
    public class GetPromotionByDiscountCodeQueryHandler : IRequestHandler<GetPromotionByDiscountCodeQuery, Result<Entities.Promotion>>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for get promotion by id request
        /// </summary>
        public GetPromotionByDiscountCodeQueryHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with promotion data</returns>
        public async Task<Result<Entities.Promotion>> Handle(GetPromotionByDiscountCodeQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Find promotion without allow null return. If promotion not found will throw NotFoundException
            var promotion = await promotionRepository.FindSingleAsync(x => x.DiscountCode == request.DiscountCode!, false, cancellationToken);
            return promotion!;
        }
    }
}
