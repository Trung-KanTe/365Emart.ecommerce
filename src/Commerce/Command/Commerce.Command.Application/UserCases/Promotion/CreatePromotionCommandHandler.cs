using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Promotion;
using Entities = Commerce.Command.Domain.Entities.Promotion;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Contants;

namespace Commerce.Command.Application.UserCases.Promotion
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreatePromotionCommand : IRequest<Result<Entities.Promotion>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DiscountCode { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}
    }

    /// <summary>
    /// Handler for create promotion request
    /// </summary>
    public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, Result<Entities.Promotion>>
    {
        private readonly IPromotionRepository promotionRepository;

        /// <summary>
        /// Handler for create promotion request
        /// </summary>
        public CreatePromotionCommandHandler(IPromotionRepository promotionRepository)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Handle create promotion request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with promotion data</returns>
        public async Task<Result<Entities.Promotion>> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
        {
            // Create new Promotion from request
            Entities.Promotion? promotion = request.MapTo<Entities.Promotion>();
            // Validate for promotion
            promotion!.ValidateCreate();
            // Begin transaction
            using var transaction = await promotionRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                var code = await promotionRepository.FindSingleAsync(x => x.DiscountCode == request.DiscountCode, true, cancellationToken);
                if (code is not null)
                {
                    return Result.Failure(StatusCode.Conflict, new Error(ErrorType.Conflict, ErrCodeConst.CONFLICT, MessConst.MSG_CONFLICT.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Promotion)) })));
                } 
                    
                // Add data
                promotionRepository.Create(promotion!);
                // Save data
                await promotionRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return promotion;
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