using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Brand;
using Entities = Commerce.Command.Domain.Entities.Brand;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;

namespace Commerce.Command.Application.UserCases.Brand
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateBrandCommand : IRequest<Result<Entities.Brand>>
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Style { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; }
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// Handler for create brand request
    /// </summary>
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result<Entities.Brand>>
    {
        private readonly IBrandRepository brandRepository;

        /// <summary>
        /// Handler for create brand request
        /// </summary>
        public CreateBrandCommandHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle create brand request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with brand data</returns>
        public async Task<Result<Entities.Brand>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            // Create new Brand from request
            Entities.Brand? brand = request.MapTo<Entities.Brand>();
            // Validate for brand
            brand!.ValidateCreate();
            // Begin transaction
            using var transaction = await brandRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                brandRepository.Create(brand!);
                // Save data
                await brandRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return brand;
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