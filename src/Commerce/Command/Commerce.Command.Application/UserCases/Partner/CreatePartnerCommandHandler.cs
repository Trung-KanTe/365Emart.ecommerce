using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Partner;
using Entities = Commerce.Command.Domain.Entities.Partner;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Partner
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreatePartnerCommand : IRequest<Result<Entities.Partner>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public bool IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for create partner request
    /// </summary>
    public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, Result<Entities.Partner>>
    {
        private readonly IPartnerRepository partnerRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create partner request
        /// </summary>
        public CreatePartnerCommandHandler(IPartnerRepository partnerRepository, IFileService fileService)
        {
            this.partnerRepository = partnerRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle create partner request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with partner data</returns>
        public async Task<Result<Entities.Partner>> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
        { 
            // Create new Partner from request
            Entities.Partner? partner = request.MapTo<Entities.Partner>();
            // Validate for partner
            //partner!.ValidateCreate();

            if (request.Icon is not null)
            {
                string relativePath = "partners";
                // Upload ảnh và lấy đường dẫn lưu trữ
                string uploadedFilePath = await fileService.UploadFile(request.Name!, request.Icon, relativePath);
                // Cập nhật đường dẫn Icon
                partner!.Icon = uploadedFilePath;
            }

            // Begin transaction
            using var transaction = await partnerRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                partnerRepository.Create(partner!);
                // Save data
                await partnerRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return partner;
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