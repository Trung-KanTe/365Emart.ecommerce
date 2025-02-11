using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using Entities = Commerce.Command.Domain.Entities.Shop;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Shop
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateShopCommand : IRequest<Result<Entities.Shop>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Style { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public int Views { get; set; } = 0;
        public string? Address { get; set; }
        public Guid? WardId { get; set; }
        public Guid? UserId { get; set; }
        public bool IsDeleted { get; set; } = true;
    }

    /// <summary>
    /// Handler for create shop request
    /// </summary>
    public class CreateShopCommandHandler : IRequestHandler<CreateShopCommand, Result<Entities.Shop>>
    {
        private readonly IShopRepository shopRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create shop request
        /// </summary>
        public CreateShopCommandHandler(IShopRepository shopRepository, IFileService fileService)
        {
            this.shopRepository = shopRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle create shop request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with shop data</returns>
        public async Task<Result<Entities.Shop>> Handle(CreateShopCommand request, CancellationToken cancellationToken)
        {
            // Create new Shop from request
            Entities.Shop? shop = request.MapTo<Entities.Shop>();
            // Validate for shop
            //shop!.ValidateCreate();

            if (request.Image is not null)
            {
                string relativePath = "shops";
                // Upload ảnh và lấy đường dẫn lưu trữ
                string uploadedFilePath = await fileService.UploadFile(request.Name!, request.Image, relativePath);
                // Cập nhật đường dẫn Icon
                shop!.Image = uploadedFilePath;
            }

            // Begin transaction
            using var transaction = await shopRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                // Add data
                shopRepository.Create(shop!);
                // Save data
                await shopRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return shop;
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