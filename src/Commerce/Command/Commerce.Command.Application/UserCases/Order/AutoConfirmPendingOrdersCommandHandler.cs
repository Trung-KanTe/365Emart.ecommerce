//using Commerce.Command.Domain.Abstractions.Repositories.Order;
//using MediatR;

//namespace Commerce.Command.Application.UserCases.Order
//{
//    public class AutoConfirmPendingOrdersCommand : IRequest
//    {
//    }

//    public class AutoConfirmPendingOrdersCommandHandler : IRequestHandler<AutoConfirmPendingOrdersCommand>
//    {
//        private readonly IOrderRepository orderRepository;

//        public AutoConfirmPendingOrdersCommandHandler(IOrderRepository orderRepository)
//        {
//            this.orderRepository = orderRepository;
//        }

//        public async Task Handle(AutoConfirmPendingOrdersCommand request, CancellationToken cancellationToken)
//        {
//            var pendingOrders = orderRepository.FindAll(x => x.Status == "pending").ToList();
//            foreach (var order in pendingOrders)
//            {
//                order.Status = "confirm";
//                orderRepository.Update(order);
//            }
//            await orderRepository.SaveChangesAsync(cancellationToken);
//        }
//    }
//}