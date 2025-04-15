using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Promotion;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Promotion;

namespace Commerce.Query.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to get all promotion
    /// </summary>
    public class GetAllPromotionQuery : IRequest<Result<List<Entities.Promotion>>>
    {
    }

    /// <summary>
    /// Handler for get all promotion request
    /// </summary>
    public class GetAllPromotionQueryHandler : IRequestHandler<GetAllPromotionQuery, Result<List<Entities.Promotion>>>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for get all promotion request
        /// </summary>
        public GetAllPromotionQueryHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list promotion as data</returns>
        public async Task<Result<List<Entities.Promotion>>> Handle(GetAllPromotionQuery request,
                                                       CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow; // Use UTC for consistency
            var activePromotions = promotionRepository.FindAll()
                                                     .Where(p => p.EndDate.HasValue && p.EndDate >= now)
                                                     .ToList();
            return await Task.FromResult(activePromotions);
        }
    }
}
