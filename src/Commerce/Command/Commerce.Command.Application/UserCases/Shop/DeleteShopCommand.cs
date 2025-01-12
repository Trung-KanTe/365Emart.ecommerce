using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using Entities = Commerce.Command.Domain.Entities.Shop;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Shop
{
    /// <summary>
    /// Request to delete shop, contain shop id
    /// </summary>
    public record DeleteShopCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete shop, contain shop id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete shop request
    /// </summary>
    public class DeleteShopCommandHandler : IRequestHandler<DeleteShopCommand, Result>
    {
        private readonly IShopRepository shopRepository;

        /// <summary>
        /// Handler for delete shop request
        /// </summary>
        public DeleteShopCommandHandler(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }

        /// <summary>
        /// Handle delete shop request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteShopCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await shopRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete shop
                var shop = await shopRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (shop == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Shop)) })));
                }
                shop.IsDeleted = false;
                shopRepository.Update(shop);
                await shopRepository.SaveChangesAsync(cancellationToken);

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