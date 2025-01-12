using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Command.Domain.Entities.ImportProduct;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateImportProductCommand : IRequest<Result<Entities.ImportProduct>>
    {
        public Guid? PartnerId { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? WareHouseId { get; set; }
        public DateTime? ImportDate { get; set; }
        public string? Note { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public ICollection<ImportProductDetails>? ImportProductDetails { get; set; }
    }

    /// <summary>
    /// Handler for create importProduct request
    /// </summary>
    public class CreateImportProductCommandHandler : IRequestHandler<CreateImportProductCommand, Result<Entities.ImportProduct>>
    {
        private readonly IImportProductRepository importProductRepository;

        /// <summary>
        /// Handler for create importProduct request
        /// </summary>
        public CreateImportProductCommandHandler(IImportProductRepository importProductRepository)
        {
            this.importProductRepository = importProductRepository;
        }

        /// <summary>
        /// Handle create importProduct request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with importProduct data</returns>
        public async Task<Result<Entities.ImportProduct>> Handle(CreateImportProductCommand request, CancellationToken cancellationToken)
        {
            // Create new ImportProduct from request
            Entities.ImportProduct? importProduct = request.MapTo<Entities.ImportProduct>();
            // Validate for importProduct
            importProduct!.ValidateCreate();
            // Begin transaction
            using var transaction = await importProductRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                importProduct!.ImportProductDetails = request.ImportProductDetails!.Select(ver => new Entities.ImportProductDetails
                {
                    ImportProductId = importProduct.Id,
                    ProductId = ver.ProductId,
                    ImportPrice = ver.ImportPrice,
                    Quantity = ver.Quantity,
                    Note = ver.Note,
                }).ToList();
                // Add data
                importProductRepository.Create(importProduct!);
                // Save data
                await importProductRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return importProduct;
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