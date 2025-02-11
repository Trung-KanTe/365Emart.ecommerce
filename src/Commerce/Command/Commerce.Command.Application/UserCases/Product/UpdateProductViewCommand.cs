using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Application.UserCases.Product
{
    public record UpdateProductViewCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete product, contain product id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete product request
    /// </summary>
    public class UpdateProductViewCommandHandler : IRequestHandler<UpdateProductViewCommand, Result>
    {
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Handler for delete product request
        /// </summary>
        public UpdateProductViewCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle delete product request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateProductViewCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await productRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete product
                var product = await productRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (product == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Product)) })));
                }
                product.Views++;
                // Mark product as Updated state
                productRepository.Update(product);
                // Save product to database
                await productRepository.SaveChangesAsync(cancellationToken);
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