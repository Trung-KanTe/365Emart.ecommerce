using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Partner;
using Entities = Commerce.Command.Domain.Entities.Partner;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Partner
{
    /// <summary>
    /// Request to delete partner, contain partner id
    /// </summary>
    public record DeletePartnerCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete partner, contain partner id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete partner request
    /// </summary>
    public class DeletePartnerCommandHandler : IRequestHandler<DeletePartnerCommand, Result>
    {
        private readonly IPartnerRepository partnerRepository;

        /// <summary>
        /// Handler for delete partner request
        /// </summary>
        public DeletePartnerCommandHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Handle delete partner request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeletePartnerCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await partnerRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete partner
                var partner = await partnerRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (partner == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error( ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Partner)) })));
                }
                partner.IsDeleted = !partner.IsDeleted;
                partnerRepository.Update(partner);
                await partnerRepository.SaveChangesAsync(cancellationToken);

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