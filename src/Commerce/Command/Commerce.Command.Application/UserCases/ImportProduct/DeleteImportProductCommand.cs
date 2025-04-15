using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to delete importProduct, contain importProduct id
    /// </summary>
    public record DeleteImportProductCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete importProduct, contain importProduct id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete importProduct request
    /// </summary>
    public class DeleteImportProductCommandHandler : IRequestHandler<DeleteImportProductCommand, Result>
    {
        private readonly IImportProductRepository importProductRepository;

        /// <summary>
        /// Handler for delete importProduct request
        /// </summary>
        public DeleteImportProductCommandHandler(IImportProductRepository importProductRepository)
        {
            this.importProductRepository = importProductRepository;
        }

        /// <summary>
        /// Handle delete importProduct request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteImportProductCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await importProductRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete importProduct
                var importProduct = await importProductRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (importProduct == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.ImportProduct)) })));
                }
                importProduct.IsDeleted = !importProduct.IsDeleted;
                importProductRepository.Update(importProduct);
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