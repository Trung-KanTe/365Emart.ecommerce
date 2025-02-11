using Commerce.Command.Contract.Shared;
using Entities = Commerce.Command.Domain.Entities.Category;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Domain.Abstractions.Repositories.Category;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Classification
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateClassificationCommand : IRequest<Result<Entities.Classification>>
    {
        public string? Name { get; set; }
        public string? Icon { get; set; } = null;
        public string? Style { get; set; } = null;
        public int? Views { get; set; } = 0;
        public bool? IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for create classification request
    /// </summary>
    public class CreateClassificationCommandHandler : IRequestHandler<CreateClassificationCommand, Result<Entities.Classification>>
    {
        private readonly IClassificationRepository classificationRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create classification request
        /// </summary>
        public CreateClassificationCommandHandler(IClassificationRepository classificationRepository, IFileService fileService)
        {
            this.classificationRepository = classificationRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle create classification request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with classification data</returns>
        public async Task<Result<Entities.Classification>> Handle(CreateClassificationCommand request, CancellationToken cancellationToken)
        {
            // Create new Classification from request
            Entities.Classification? classification = request.MapTo<Entities.Classification>();
            // Validate for classification
            //classification!.ValidateCreate();

            if (request.Icon is not null)
            {
                string relativePath = "classifications";
                // Upload ảnh và lấy đường dẫn lưu trữ
                string uploadedFilePath = await fileService.UploadFile(request.Name!, request.Icon, relativePath);
                // Cập nhật đường dẫn Icon
                classification!.Icon = uploadedFilePath;
            }
            // Begin transaction
            using var transaction = await classificationRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                classificationRepository.Create(classification!);
                // Save data
                await classificationRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return classification;
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