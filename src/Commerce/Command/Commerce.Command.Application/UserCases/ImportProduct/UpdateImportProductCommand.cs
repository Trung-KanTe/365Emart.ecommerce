using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;
using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using MediatR;
using Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to delete importProduct, contain importProduct id
    /// </summary>
    public record UpdateImportProductCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
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
    /// Handler for delete importProduct request
    /// </summary>
    public class UpdateImportProductCommandHandler : IRequestHandler<UpdateImportProductCommand, Result>
    {
        private readonly IImportProductRepository importProductRepository;

        /// <summary>
        /// Handler for delete importProduct request
        /// </summary>
        public UpdateImportProductCommandHandler(IImportProductRepository importProductRepository)
        {
            this.importProductRepository = importProductRepository;
        }

        /// <summary>
        /// Handle delete importProduct request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateImportProductCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await importProductRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete importProduct
                var importProduct = await importProductRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (importProduct == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.ImportProduct)) })));
                }
                // Update importProduct, keep original data if request is null
                request.MapTo(importProduct, true);
                importProduct!.ImportProductDetails = request.ImportProductDetails?.Distinct().Select(ver => new Entities.ImportProductDetails
                {
                    ImportProductId = importProduct.Id,
                    ProductId = ver.ProductId,
                    ImportPrice = ver.ImportPrice,
                    Quantity = ver.Quantity,
                    Note = ver.Note,
                }).ToList() ?? importProduct.ImportProductDetails;
                // Mark importProduct as Updated state
                importProductRepository.Update(importProduct);
                // Save importProduct to database
                await importProductRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return Result.Ok();
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