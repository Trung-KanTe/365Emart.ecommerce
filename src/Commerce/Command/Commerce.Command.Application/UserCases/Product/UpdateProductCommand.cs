using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using MediatR;

namespace Commerce.Command.Application.UserCases.Product
{
    /// <summary>
    /// Request to delete product, contain product id
    /// </summary>
    public record UpdateProductCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int? Views { get; set; } = 0;
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ShopId { get; set; }
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for delete product request
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Handler for delete product request
        /// </summary>
        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Handle delete product request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await productRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete product
                var product = await productRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (product == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Product)) })));
                }
                // Update product, keep original data if request is null
                request.MapTo(product, true);
                product.ValidateUpdate();
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