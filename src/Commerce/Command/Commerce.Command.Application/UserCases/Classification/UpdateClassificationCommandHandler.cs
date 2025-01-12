using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Category;

namespace Commerce.Command.Application.UserCases.Classification
{
    /// <summary>
    /// Request to delete classification, contain classification id
    /// </summary>
    public record UpdateClassificationCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; } = null;
        public string? Style { get; set; } = null;
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Handler for delete classification request
    /// </summary>
    public class UpdateClassificationCommandHandler : IRequestHandler<UpdateClassificationCommand, Result>
    {
        private readonly IClassificationRepository classificationRepository;

        /// <summary>
        /// Handler for delete classification request
        /// </summary>
        public UpdateClassificationCommandHandler(IClassificationRepository classificationRepository)
        {
            this.classificationRepository = classificationRepository;
        }

        /// <summary>
        /// Handle delete classification request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateClassificationCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await classificationRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete classification
                var classification = await classificationRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (classification == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Classification)) })));
                }
                // Update classification, keep original data if request is null
                request.MapTo(classification, true);
                classification.ValidateUpdate();
                // Mark classification as Updated state
                classificationRepository.Update(classification);
                // Save classification to database
                await classificationRepository.SaveChangesAsync(cancellationToken);
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