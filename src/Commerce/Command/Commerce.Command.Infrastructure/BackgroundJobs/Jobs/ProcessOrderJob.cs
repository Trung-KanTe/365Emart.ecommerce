//using Commerce.Command.Application.UserCases.Order;
//using MediatR;
//using Microsoft.Extensions.DependencyInjection;

//namespace Commerce.Command.Infrastructure.BackgroundJobs.Jobs
//{
//    public class ProcessOrderJob
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;

//        public ProcessOrderJob(IServiceScopeFactory serviceScopeFactory)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//        }

//        public async Task AutoConfirmAllPendingOrders()
//        {
//            using var scope = _serviceScopeFactory.CreateScope();
//            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

//            await mediator.Send(new AutoConfirmPendingOrdersCommand());
//        }

//        public static void Run(IServiceProvider serviceProvider)
//        {
//            using var scope = serviceProvider.CreateScope();
//            var job = scope.ServiceProvider.GetRequiredService<ProcessOrderJob>();
//            job.AutoConfirmAllPendingOrders().Wait(); 
//        }
//    }
//}