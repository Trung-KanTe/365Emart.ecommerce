using Commerce.Command.Domain.Constants.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Persistence.Configurations.Payment
{
    /// <summary>
    /// EF Core configuration for PaymentDetails entity
    /// </summary>
    public class PaymentDetailsConfig : IEntityTypeConfiguration<Entities.PaymentDetails>
    {
        public void Configure(EntityTypeBuilder<Entities.PaymentDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_ID);

            builder.Property(x => x.PaymentId)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_PAYMENT_ID)
                .IsRequired();

            builder.Property(x => x.BankCode)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_TRANSACTION_CODE)
                .HasMaxLength(PaymentConst.PAYMENT_DETAILS_TRANSACTION_CODE_MAX_LENGTH)
                .IsRequired();

            builder.Property(x => x.BankName)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_BANK_NAME)
                .HasMaxLength(PaymentConst.PAYMENT_DETAILS_BANK_NAME_MAX_LENGTH);

            builder.Property(x => x.CardNumber)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_CARD_NUMBER)
                .HasMaxLength(PaymentConst.PAYMENT_DETAILS_CARD_NUMBER_MAX_LENGTH);

            builder.Property(x => x.Note)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_NOTE)
                .HasMaxLength(PaymentConst.PAYMENT_DETAILS_NOTE_MAX_LENGTH);

            builder.Property(x => x.ExtraData)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DETAILS_EXTRA_DATA);

            builder.ToTable(PaymentConst.TABLE_PAYMENT_DETAILS);
            builder.HasOne(mer => mer.Payment)
                   .WithMany(ver => ver.PaymentDetails)
                   .HasForeignKey(mer => mer.PaymentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
