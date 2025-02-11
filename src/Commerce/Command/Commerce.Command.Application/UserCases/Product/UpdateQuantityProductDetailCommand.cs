using Commerce.Command.Application.UserCases.DTOs;
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
    public record UpdateQuantityProductDetailCommand : IRequest<Result>
    {  
        public List<ProductDetailDTO>? ProductDetail { get; set; }
    }

    /// <summary>
    /// Handler for delete product request
    /// </summary>
    public class UpdateQuantityProductDetailCommandHandler : IRequestHandler<UpdateQuantityProductDetailCommand, Result>
    {
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for delete product request
        /// </summary>
        public UpdateQuantityProductDetailCommandHandler(IProductDetailRepository productDetailRepository)
        {
            this.productDetailRepository = productDetailRepository;
        }

        /// <summary>
        /// Handle delete product request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateQuantityProductDetailCommand request, CancellationToken cancellationToken)
        {
            var productDetail = request.MapTo<ProductDetailDTO>();
            using var transaction = await productDetailRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var item in request.ProductDetail!)
                {
                    var product = await productDetailRepository.FindByIdAsync(item.Id!.Value, true, cancellationToken);
                    if (product == null)
                    {
                        return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Product)) })));
                    }
                    product.StockQuantity += item.StockQuantity;
                    // Mark product as Updated state
                    productDetailRepository.Update(product);
                }               
                // Save product to database
                await productDetailRepository.SaveChangesAsync(cancellationToken);
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