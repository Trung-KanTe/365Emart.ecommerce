using Commerce.Query.Contract.Shared;
using Commerce.Query.Domain.Abstractions.Repositories.Order;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.Order;

namespace Commerce.Query.Application.UserCases.Order
{
    /// <summary>
    /// Request to get all orderCancel
    /// </summary>
    public class GetAllOrderCancelQuery : IRequest<Result<List<Entities.OrderCancel>>>
    {
    }

    /// <summary>
    /// Handler for get all orderCancel request
    /// </summary>
    public class GetAllOrderCancelQueryHandler : IRequestHandler<GetAllOrderCancelQuery, Result<List<Entities.OrderCancel>>>
    {
        private readonly IOrderCancelRepository orderCancelRepository;

        /// <summary>
        /// Handler for get all orderCancel request
        /// </summary>
        public GetAllOrderCancelQueryHandler(IOrderCancelRepository orderCancelRepository)
        {
            this.orderCancelRepository = orderCancelRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with list orderCancel as data</returns>
        public async Task<Result<List<Entities.OrderCancel>>> Handle(GetAllOrderCancelQuery request,
                                                       CancellationToken cancellationToken)
        {
            var orderCancels = orderCancelRepository.FindAll().ToList();
            return await Task.FromResult(orderCancels);
        }
    }
}
