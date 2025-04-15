using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Cart;
using Entities = Commerce.Command.Domain.Entities.Cart;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Cart
{
    /// <summary>
    /// Request to delete cart, contain cart id
    /// </summary>
    public record DeleteCartCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete cart, contain cart id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete cart request
    /// </summary>
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, Result>
    {
        private readonly ICartRepository cartRepository;

        /// <summary>
        /// Handler for delete cart request
        /// </summary>
        public DeleteCartCommandHandler(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        /// <summary>
        /// Handle delete cart request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await cartRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete cart
                var cart = await cartRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (cart == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Cart)) })));
                }
                cart.IsDeleted = !cart.IsDeleted;
                cartRepository.Update(cart);
                await cartRepository.SaveChangesAsync(cancellationToken);

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