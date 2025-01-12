using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.WareHouse;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.WareHouse;

namespace Commerce.Query.Application.WareHouseCases.WareHouse
{
    /// <summary>
    /// Request to get wareHouse by id
    /// </summary>
    public record GetWareHouseByIdQuery : IRequest<Result<Entities.WareHouse>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get wareHouse by id request
    /// </summary>
    public class GetWareHouseByIdQueryHandler : IRequestHandler<GetWareHouseByIdQuery, Result<Entities.WareHouse>>
    {
        private readonly IWareHouseRepository wareHouseRepository;

        /// <summary>
        /// Handler for get wareHouse by id request
        /// </summary>
        public GetWareHouseByIdQueryHandler(IWareHouseRepository wareHouseRepository)
        {
            this.wareHouseRepository = wareHouseRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with wareHouse data</returns>
        public async Task<Result<Entities.WareHouse>> Handle(GetWareHouseByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            // Find wareHouse without allow null return. If wareHouse not found will throw NotFoundException
            var wareHouse = await wareHouseRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);
            return wareHouse!;
        }
    }
}
