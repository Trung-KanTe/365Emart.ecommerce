﻿using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.ImportProduct;
using Commerce.Command.Domain.Abstractions.Repositories.ProducStock;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Entities.ImportProduct;
using Commerce.Command.Domain.Entities.ProductStock;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.ImportProduct;

namespace Commerce.Command.Application.UserCases.ImportProduct
{
    /// <summary>
    /// Request to create
    /// </summary>
    public record CreateImportProductCommand : IRequest<Result<Entities.ImportProduct>>
    {
        public Guid? PartnerId { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? WareHouseId { get; set; }
        public string? Note { get; set; }
        public bool? IsDeleted { get; set; } = true;
        public ICollection<ImportProductDetails>? ImportProductDetails { get; set; }
    }

    /// <summary>
    /// Handler for create importProduct request
    /// </summary>
    public class CreateImportProductCommandHandler : IRequestHandler<CreateImportProductCommand, Result<Entities.ImportProduct>>
    {
        private readonly IImportProductRepository importProductRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IProductDetailRepository productDetailRepository;

        /// <summary>
        /// Handler for create importProduct request
        /// </summary>
        public CreateImportProductCommandHandler(IImportProductRepository importProductRepository, IProductStockRepository productStockRepository, IProductDetailRepository productDetailRepository)
        {
            this.importProductRepository = importProductRepository;
            this.productStockRepository = productStockRepository;
            this.productDetailRepository = productDetailRepository;
        }

        /// <summary>
        /// Handle create importProduct request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result with importProduct data</returns>
        public async Task<Result<Entities.ImportProduct>> Handle(CreateImportProductCommand request, CancellationToken cancellationToken)
        {
            // Create new ImportProduct from request
            Entities.ImportProduct? importProduct = request.MapTo<Entities.ImportProduct>();
            // Validate for importProduct
            importProduct!.ValidateCreate();
            // Begin transaction
            using var transaction = await importProductRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                importProduct!.ImportProductDetails = request.ImportProductDetails!.Select(ver => new Entities.ImportProductDetails
                {
                    ImportProductId = importProduct.Id,
                    ProductDetailId = ver.ProductDetailId,
                    ImportPrice = ver.ImportPrice,
                    Quantity = ver.Quantity,
                }).ToList();

                foreach (var item in importProduct.ImportProductDetails) 
                {
                    var stockExist = await productStockRepository.FindSingleAsync(x => x.ProductDetailId == item.ProductDetailId && x.WareHouseId == importProduct.WareHouseId, true, cancellationToken);
                    if (stockExist != null)
                    {
                        stockExist.Quantity += item.Quantity;
                        productStockRepository.Update(stockExist);
                    }
                    else
                    {
                        var stock = new ProductStock
                        {
                            ProductDetailId = item.ProductDetailId,
                            WareHouseId = importProduct.WareHouseId,
                            Quantity = item.Quantity,
                        };
                        productStockRepository.Create(stock);
                    }

                    var stockProduct = await productDetailRepository.FindByIdAsync(item.ProductDetailId!.Value, true, cancellationToken);
                    stockProduct!.StockQuantity += item.Quantity;
                    productDetailRepository.Update(stockProduct);
                    await productDetailRepository.SaveChangesAsync(cancellationToken);
                }
                // Add data
                importProductRepository.Create(importProduct!);
                // Save data
                await importProductRepository.SaveChangesAsync(cancellationToken);
                await productStockRepository.SaveChangesAsync(cancellationToken);               
                // Commit transaction
                transaction.Commit();
                return importProduct;
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