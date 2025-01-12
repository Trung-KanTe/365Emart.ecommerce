using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Entities = Commerce.Command.Domain.Entities.Promotion;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to delete promotion, contain promotion id
    /// </summary>
    public record DeletePromotionCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete promotion, contain promotion id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete promotion request
    /// </summary>
    public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand, Result>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for delete promotion request
        /// </summary>
        public DeletePromotionCommandHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle delete promotion request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await promotionRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete promotion
                var promotion = await promotionRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (promotion == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Promotion)) })));
                }
                promotion.IsDeleted = false;
                promotionRepository.Update(promotion);
                await promotionRepository.SaveChangesAsync(cancellationToken);

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