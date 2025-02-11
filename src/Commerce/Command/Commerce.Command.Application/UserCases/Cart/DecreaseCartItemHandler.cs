using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Cart;

namespace Commerce.Command.Application.UserCases.Cart
{
    /// <summary>
    /// Request to update cartItem, contain cartItem id
    /// </summary>
    public record DecreaseCartItemCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to update cartItem, contain cartItem id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for update cartItem request
    /// </summary>
    public class DecreaseCartItemHandler : IRequestHandler<DecreaseCartItemCommand, Result>
    {
        private readonly ICartItemRepository cartItemRepository;
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Handler for update cartItem request
        /// </summary>
        public DecreaseCartItemHandler(ICartItemRepository cartItemRepository, ICartRepository cartRepository)
        {
            this.cartItemRepository = cartItemRepository;
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Handle update cartItem request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether update action success or not</returns>
        public async Task<Result> Handle(DecreaseCartItemCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await cartItemRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to update cartItem
                var cartItem = await cartItemRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (cartItem == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.CartItem)) })));
                }
                cartItem.Quantity--;
                cartItem.Total = cartItem.Quantity * cartItem.Price;
                cartItemRepository.Update(cartItem);
                await cartItemRepository.SaveChangesAsync(cancellationToken);

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