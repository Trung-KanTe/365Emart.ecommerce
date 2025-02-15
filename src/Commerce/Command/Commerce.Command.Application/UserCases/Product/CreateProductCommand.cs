using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Entities = Commerce.Command.Domain.Entities.Product;
using MediatR;
using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Domain.Entities.Product;
using Commerce.Command.Contract.Abstractions;

namespace Commerce.Command.Application.UserCases.Product
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateProductCommand : IRequest<Result<Entities.Product>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Views { get; set; } = 0;
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ShopId { get; set; }
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; } = true;
        public ICollection<ProductDetail>? ProductDetails { get; set; }
    }

    /// <summary>
    /// Handler for create product request
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Entities.Product>>
    {
        private readonly IProductRepository productRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Handler for create product request
        /// </summary>
        public CreateProductCommandHandler(IProductRepository productRepository, IFileService fileService)
        {
            this.productRepository = productRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Handle create product request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with product data</returns>
        public async Task<Result<Entities.Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Create new Product from request
            Entities.Product? product = request.MapTo<Entities.Product>();
            // Validate for product
            if (request.Image is not null)
            {
                string normalizedProductName = request.Name!.Replace(" ", "_");
                string relativePath = "products";
                string uploadedFilePath = await fileService.UploadFile(normalizedProductName, request.Image, relativePath);
                product!.Image = uploadedFilePath;
            }
            //product!.ValidateCreate();
            // Begin transaction
            using var transaction = await productRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                product!.ProductDetails = request.ProductDetails!.Select(ver => new ProductDetail
                {
                    ProductId = product.Id,
                    Size = ver.Size,
                    StockQuantity = 0,
                    Color = ver.Color,
                }).ToList();
                // Add data
                productRepository.Create(product!);
                // Save data
                await productRepository.SaveChangesAsync(cancellationToken);
                // Commit transaction
                transaction.Commit();
                return product;
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