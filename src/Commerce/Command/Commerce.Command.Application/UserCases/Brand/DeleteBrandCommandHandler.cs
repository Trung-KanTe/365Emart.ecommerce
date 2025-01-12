using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.Brand;
using Entities = Commerce.Command.Domain.Entities.Brand;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.Brand
{
    /// <summary>
    /// Request to delete brand, contain brand id
    /// </summary>
    public record DeleteBrandCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete brand, contain brand id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete brand request
    /// </summary>
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result>
    {
        private readonly IBrandRepository brandRepository;

        /// <summary>
        /// Handler for delete brand request
        /// </summary>
        public DeleteBrandCommandHandler(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        /// <summary>
        /// Handle delete brand request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await brandRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete brand
                var brand = await brandRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (brand == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Brand)) })));
                }
                brand.IsDeleted = false;
                brandRepository.Update(brand);
                await brandRepository.SaveChangesAsync(cancellationToken);

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