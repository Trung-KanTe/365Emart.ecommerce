using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Commerce.Command.Domain.Abstractions.Repositories.WareHouse;
using Entities = Commerce.Command.Domain.Entities.WareHouse;
using MediatR;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;

namespace Commerce.Command.Application.UserCases.WareHouse
{
    /// <summary>
    /// Request to delete wareHouse, contain wareHouse id
    /// </summary>
    public record DeleteWareHouseCommand : IRequest<Result>
    {
        /// <summary>
        /// Request to delete wareHouse, contain wareHouse id
        /// </summary>
        public Guid? Id { get; set; }
    }

    /// <summary>
    /// Handler for delete wareHouse request
    /// </summary>
    public class DeleteWareHouseCommandHandler : IRequestHandler<DeleteWareHouseCommand, Result>
    {
        private readonly IWareHouseRepository wareHouseRepository;

        /// <summary>
        /// Handler for delete wareHouse request
        /// </summary>
        public DeleteWareHouseCommandHandler(IWareHouseRepository wareHouseRepository)
        {
            this.wareHouseRepository = wareHouseRepository;
        }

        /// <summary>
        /// Handle delete wareHouse request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(DeleteWareHouseCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await wareHouseRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete wareHouse
                var wareHouse = await wareHouseRepository.FindByIdAsync(request.Id!.Value, true, cancellationToken);
                if (wareHouse == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.WareHouse)) })));
                }
                wareHouse.IsDeleted = !wareHouse.IsDeleted;
                wareHouseRepository.Update(wareHouse);
                await wareHouseRepository.SaveChangesAsync(cancellationToken);

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