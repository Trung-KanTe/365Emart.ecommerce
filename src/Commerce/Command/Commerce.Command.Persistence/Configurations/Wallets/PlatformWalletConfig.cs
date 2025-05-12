using Commerce.Command.Domain.Constants.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Wallets;

namespace Commerce.Command.Persistence.Configurations.Wallets
{
    public class PlatformWalletConfig : IEntityTypeConfiguration<Entities.PlatformWallet>
    {
        public void Configure(EntityTypeBuilder<Entities.PlatformWallet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_ID);

            builder.Property(x => x.Balance)
                   .HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_BALANCE)
                   .IsRequired();

            builder.Property(x => x.InsertedAt).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_INSERTED_AT);
            builder.Property(x => x.InsertedBy).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_INSERTED_BY);
            builder.Property(x => x.UpdatedAt).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_UPDATED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(WalletConst.FIELD_PLATFORM_WALLET_UPDATED_BY);

            builder.ToTable(WalletConst.TABLE_PLATFORM_WALLET);
        }
    }
}
