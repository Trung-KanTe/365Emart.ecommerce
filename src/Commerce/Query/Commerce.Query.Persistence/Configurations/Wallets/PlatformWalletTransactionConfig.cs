using Commerce.Query.Domain.Constants.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.Wallets;

namespace Commerce.Query.Persistence.Configurations.Wallets
{
    public class PlatformWalletTransactionConfig : IEntityTypeConfiguration<Entities.PlatformWalletTransaction>
    {
        public void Configure(EntityTypeBuilder<Entities.PlatformWalletTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_ID);

            builder.Property(x => x.OrderId)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_ORDER_ID);

            builder.Property(x => x.Amount)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_AMOUNT)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_TYPE)
                   .HasMaxLength(WalletConst.WALLET_TRANS_TYPE_MAX_LENGTH)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_DESCRIPTION)
                   .HasMaxLength(WalletConst.WALLET_TRANS_DESCRIPTION_MAX_LENGTH);

            builder.Property(x => x.CreatedAt)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_TRANS_CREATED_AT)
                   .IsRequired();

            builder.ToTable(WalletConst.TABLE_PLATFORM_WALLET_TRANSACTION);
        }
    }
}
