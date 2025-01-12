using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Category;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Category;

namespace Commerce.Query.Application.UserCases.Classification
{
    /// <summary>
    /// Request to get all classification
    /// </summary>
    public class GetAllClassificationQuery : IRequest<Result<List<Entities.Classification>>>
    {
    }

    /// <summary>
    /// Handler for get all classification request
    /// </summary>
    public class GetAllClassificationQueryHandler : IRequestHandler<GetAllClassificationQuery, Result<List<Entities.Classification>>>
    {
        private readonly IClassificationRepository classificationRepository;

        /// <summary>
        /// Handler for get all classification request
        /// </summary>
        public GetAllClassificationQueryHandler(IClassificationRepository classificationRepository)
        {
            this.classificationRepository = classificationRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list classification as data</returns>
        public async Task<Result<List<Entities.Classification>>> Handle(GetAllClassificationQuery request,
                                                       CancellationToken cancellationToken)
        {
            var classifications = classificationRepository.FindAll().ToList();
            return await Task.FromResult(classifications);
        }
    }
}
