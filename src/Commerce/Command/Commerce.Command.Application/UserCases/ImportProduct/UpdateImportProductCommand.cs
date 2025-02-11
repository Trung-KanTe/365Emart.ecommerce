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
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Entities.ProductStock;

namespace Commerce.Command.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to delete importProduct, contain importProduct id
    /// </summary>
    public record UpdateImportProductCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? WareHouseId { get; set; }
        public string? Note { get; set; }
        public bool? IsDeleted { get; set; } 
        public ICollection<ImportProductDetails>? ImportProductDetails { get; set; }
    }

    /// <summary>
    /// Handler for delete importProduct request
    /// </summary>
    public class UpdateImportProductCommandHandler : IRequestHandler<UpdateImportProductCommand, Result>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IProductStockRepository productStockRepository;

        /// <summary>
        /// Handler for delete importProduct request
        /// </summary>
        public UpdateImportProductCommandHandler(IImportProductRepository importProductRepository, IProductStockRepository productStockRepository)
        {
            this.importProductRepository = importProductRepository;
            this.productStockRepository = productStockRepository;
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
                var importProduct = await importProductRepository.FindByIdAsync(request.Id.Value, true, cancellationToken, x=>x.ImportProductDetails!);
                if (importProduct == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.ImportProduct)) })));
                }
                // Update importProduct, keep original data if request is null
                request.MapTo(importProduct, true);
                importProduct!.ImportProductDetails = request.ImportProductDetails!.Select(ver => new Entities.ImportProductDetails
                {
                    ImportProductId = importProduct.Id,
                    ProductId = ver.ProductId,
                    ImportPrice = ver.ImportPrice,
                    Quantity = ver.Quantity,
                }).ToList();

                foreach (var item in importProduct.ImportProductDetails)
                {
                    var stockExist = await productStockRepository.FindSingleAsync(x => x.ProductId == item.ProductId && x.WareHouseId == importProduct.WareHouseId, true, cancellationToken);
                    if (stockExist != null)
                    {
                        stockExist.Quantity = stockExist.Quantity + item.Quantity;
                        productStockRepository.Update(stockExist);
                    }
                    else
                    {
                        var stock = new ProductStock
                        {
                            ProductId = item.ProductId,
                            WareHouseId = importProduct.WareHouseId,
                            Quantity = item.Quantity,
                        };
                        productStockRepository.Create(stock);
                    }
                }
                // Mark importProduct as Updated state
                importProductRepository.Update(importProduct);
                // Save importProduct to database
                await productStockRepository.SaveChangesAsync(cancellationToken);
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