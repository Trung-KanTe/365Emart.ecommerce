using Commerce.Query.Application.DTOs;
using Commerce.Query.Contract.DependencyInjection.Extensions;
using Commerce.Query.Contract.Shared;
using Commerce.Query.Contract.Validators;
using Commerce.Query.Domain.Abstractions.Repositories.ImportProduct;
using MediatR;
using Entities = Commerce.Query.Domain.Entities.ImportProduct;

namespace Commerce.Query.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to get importProduct by id
    /// </summary>
    public record GetImportProductByIdQuery : IRequest<Result<ImportProductDTO>>
    {
        public Guid? Id { get; init; }
    }

    /// <summary>
    /// Handler for get importProduct by id request
    /// </summary>
    public class GetImportProductByIdQueryHandler : IRequestHandler<GetImportProductByIdQuery, Result<ImportProductDTO>>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IImportProductDetailRepository importProductDetailRepository;

        /// <summary>
        /// Handler for get importProduct by id request
        /// </summary>
        public GetImportProductByIdQueryHandler(IImportProductRepository importProductRepository, IImportProductDetailRepository importProductDetailRepository)
        {
            this.importProductRepository = importProductRepository;
            this.importProductDetailRepository = importProductDetailRepository;
        }

        /// <summary>
        /// Handle request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with importProduct data</returns>
        public async Task<Result<ImportProductDTO>> Handle(GetImportProductByIdQuery request,
                                                 CancellationToken cancellationToken)
        {
            // Create validator for request 
            var validator = Validator.Create(request);
            // Setup rule for request id that must be greater than 0
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            // Validate request
            validator.Validate();

            var importProduct = await importProductRepository.FindByIdAsync(request.Id!.Value, false, cancellationToken);          
            ImportProductDTO importProductDto = importProduct!.MapTo<ImportProductDTO>()!;
            importProductDto.ImportProductDetails = importProductDetailRepository.FindAll(x => x.ImportProductId == request.Id).ToList().Select(x => x.MapTo<Entities.ImportProductDetails>()!).ToList();
            return importProductDto;
        }
    }
}
