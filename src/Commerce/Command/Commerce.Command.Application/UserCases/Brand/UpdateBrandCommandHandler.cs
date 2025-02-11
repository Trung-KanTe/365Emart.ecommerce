using Commerce.Command.Contract.Contants;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Enumerations;
using Commerce.Command.Contract.Errors;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Contract.Validators;
using Entities = Commerce.Command.Domain.Entities.Brand;
using Commerce.Command.Domain.Abstractions.Repositories.Brand;
using MediatR;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Brand
{
    /// <summary>
    /// Request to delete brand, contain brand id
    /// </summary>
    public record UpdateBrandCommand : IRequest<Result>
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public string? Style { get; set; }
        public Guid? UserId { get; set; }
        public int? Views { get; set; }
        public bool? IsDeleted { get; set; }
    }

    /// <summary>
    /// Handler for delete brand request
    /// </summary>
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Result>
    {
        private readonly IBrandRepository brandRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for delete brand request
        /// </summary>
        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IFileService fileService)
        {
            this.brandRepository = brandRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle delete brand request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result indicate whether delete action success or not</returns>
        public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var validator = Validator.Create(request);
            validator.RuleFor(x => x.Id).NotNull().IsGuid();
            validator.Validate();
            using var transaction = await brandRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Need tracking to delete brand
                var brand = await brandRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
                if (brand == null)
                {
                    return Result.Failure(StatusCode.NotFound, new Error(ErrorType.NotFound, ErrCodeConst.NOT_FOUND, MessConst.NOT_FOUND.FillArgs(new List<MessageArgs> { new MessageArgs(Args.TABLE_NAME, nameof(Entities.Brand)) })));
                }
                // Update brand, keep original data if request is null
                request.MapTo(brand, true);
                if (request.Icon is not null)
                {
                    string relativePath = "brands";
                    // Upload ảnh và lấy đường dẫn lưu trữ
                    string uploadedFilePath = await fileService.UploadFile(brand.Name!, request.Icon, relativePath);
                    // Cập nhật đường dẫn Icon
                    brand!.Icon = uploadedFilePath;
                }
                //brand.ValidateUpdate();
                // Mark brand as Updated state
                brandRepository.Update(brand);
                // Save brand to database
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