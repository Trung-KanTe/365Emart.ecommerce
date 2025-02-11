using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Promotion;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using MediatR;

namespace Commerce.Command.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to delete promotion, contain promotion id
    /// </summary>
    public record UpdatePromotionCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DiscountCode { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// Handler for delete promotion request
    /// </summary>
    public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, Result>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for delete promotion request
        /// </summary>
        public UpdatePromotionCommandHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle delete promotion request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await promotionRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete promotion
                var promotion = await promotionRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (promotion == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Promotion)) })));
                }
                // Update promotion, keep original data if request is null
                request.MapTo(promotion, true);
                promotion.ValidateUpdate();
                // Mark promotion as Updated state
                promotionRepository.Update(promotion);
                // Save promotion to database
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