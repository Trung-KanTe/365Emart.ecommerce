using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Shop;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Shop
{
    /// <summary>
    /// Request to delete shop, contain shop id
    /// </summary>
    public record UpdateShopCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public int Views { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public Guid? UserId { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Handler for delete shop request
    /// </summary>
    public class UpdateShopCommandHandler : IRequestHandler<UpdateShopCommand, Result>
    {
        private readonly IShopRepository shopRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for delete shop request
        /// </summary>
        public UpdateShopCommandHandler(IShopRepository shopRepository, IFileService fileService)
        {
            this.shopRepository = shopRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle delete shop request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateShopCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await shopRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete shop
                var shop = await shopRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (shop == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Shop)) })));
                }
                // Update shop, keep original data if request is null
                request.MapTo(shop, true);
                if (request.Image is not null)
                {
                    string relativePath = "shops";
                    // Upload ảnh và lấy đường dẫn lưu trữ
                    string uploadedFilePath = await fileService.UploadFile(shop.Name!, request.Image, relativePath);
                    // Cập nhật đường dẫn Icon
                    shop!.Image = uploadedFilePath;
                }
                //shop.ValidateUpdate();
                // Mark shop as Updated state
                shopRepository.Update(shop);
                // Save shop to database
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