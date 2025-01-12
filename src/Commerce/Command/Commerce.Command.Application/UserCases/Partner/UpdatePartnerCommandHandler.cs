using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Partner;
using Commerce.Command.Domain.Abstractions.Repositories.Partner;
using MediatR;

namespace Commerce.Command.Application.UserCases.Partner
{
    /// <summary>
    /// Request to delete partner, contain partner id
    /// </summary>
    public record UpdatePartnerCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Handler for delete partner request
    /// </summary>
    public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand, Result>
    {
        private readonly IPartnerRepository partnerRepository;

        /// <summary>
        /// Handler for delete partner request
        /// </summary>
        public UpdatePartnerCommandHandler(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Handle delete partner request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await partnerRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete partner
                var partner = await partnerRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (partner == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Partner)) })));
                }
                // Update partner, keep original data if request is null
                request.MapTo(partner, true);
                partner.ValidateUpdate();
                // Mark partner as Updated state
                partnerRepository.Update(partner);
                // Save partner to database
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