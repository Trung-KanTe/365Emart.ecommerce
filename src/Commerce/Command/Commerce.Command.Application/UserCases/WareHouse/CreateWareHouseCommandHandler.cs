using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.WareHouse;
using Entities = Commerce.Command.Domain.Entities.WareHouse;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;

namespace Commerce.Command.Application.UserCases.WareHouse
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateWareHouseCommand : IRequest<Result<Entities.WareHouse>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public bool IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for create wareHouse request
    /// </summary>
    public class CreateWareHouseCommandHandler : IRequestHandler<CreateWareHouseCommand, Result<Entities.WareHouse>>
    {
        private readonly IWareHouseRepository wareHouseRepository;

        /// <summary>
        /// Handler for create wareHouse request
        /// </summary>
        public CreateWareHouseCommandHandler(IWareHouseRepository wareHouseRepository)
        {
            this.wareHouseRepository = wareHouseRepository;
        }

        /// <summary>
        /// Handle create wareHouse request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with wareHouse data</returns>
        public async Task<Result<Entities.WareHouse>> Handle(CreateWareHouseCommand request, CancellationToken cancellationToken)
        {
            // Create new WareHouse from request
            Entities.WareHouse? wareHouse = request.MapTo<Entities.WareHouse>();
            // Validate for wareHouse
            //wareHouse!.ValidateCreate();
            // Begin transaction
            using var transaction = await wareHouseRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                wareHouseRepository.Create(wareHouse!);
                // Save data
                await wareHouseRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return wareHouse;
            }
            catch (Exception)
            {
                // Rollback transaction
                transaction.Rollback();
                throw;
            }
        }
    }
}