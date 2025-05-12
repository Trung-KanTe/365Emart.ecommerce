using Commerce.Command.Contract.DependencyInjection.Extensions;
using Commerce.Command.Contract.Shared;
using Commerce.Command.Domain.Abstractions.Repositories.Product;
using Commerce.Command.Domain.Abstractions.Repositories.Shop;
using MediatR;
using Entities = Commerce.Command.Domain.Entities.Product;

namespace Commerce.Command.Application.UserCases.Product
{
    public record CreateProductReviewReplyCommand : IRequest<Result<Entities.ProductReviewReply>>
    {
        public Guid? ReviewId { get; set; }
        public Guid? UserId { get; set; }
        public string? Reply { get; set; }
        public bool? IsDeleted { get; set; } = true;
    }

    public class CreateProductReviewReplyCommandHandler : IRequestHandler<CreateProductReviewReplyCommand, Result<Entities.ProductReviewReply>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IProductReviewReplyRepository replyRepository;
        private readonly IShopRepository shopRepository;

        public CreateProductReviewReplyCommandHandler(
            IProductReviewRepository productReviewRepository,
            IProductReviewReplyRepository replyRepository,
            IShopRepository shopRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.replyRepository = replyRepository;
            this.shopRepository = shopRepository;
        }

        public async Task<Result<Entities.ProductReviewReply>> Handle(CreateProductReviewReplyCommand request, CancellationToken cancellationToken)
        {           
            Entities.ProductReviewReply reply = request.MapTo<Entities.ProductReviewReply>();
            reply.Reply = request.Reply;
            reply.ReviewId = request.ReviewId;

            var shop = await shopRepository.FindSingleAsync(x => x.UserId == request.UserId, true, cancellationToken);
            reply.ShopId = shop.Id;

            using var transaction = await replyRepository.BeginTransactionAsync(cancellationToken);
            try
            {
                replyRepository.Create(reply);
                await replyRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();
                return reply;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}