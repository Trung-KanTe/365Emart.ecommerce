using Commerce.Command.Domain.Constants.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Command.Domain.Entities.Payment;

namespace Commerce.Command.Persistence.Configurations.Payment
{
    /// <summary>
    /// EF Core configuration for Payment entity
    /// </summary>
    public class PaymentConfig : IEntityTypeConfiguration<Entities.Payment>
    {
        public void Configure(EntityTypeBuilder<Entities.Payment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(PaymentConst.FIELD_PAYMENT_ID);

            builder.Property(x => x.OrderId)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_ORDER_ID)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_AMOUNT)
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

            builder.Property(x => x.ReturnUrl)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_URL)
                .IsRequired();

            builder.Property(x => x.OrderInfo)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_ORDER_INFO)
                .IsRequired();

            builder.Property(x => x.IpAddress)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_IPADDRESS)
                .IsRequired();

            builder.Property(x => x.TransactionId)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_TRANSACTION_ID)
                .HasMaxLength(PaymentConst.PAYMENT_TRANSACTION_ID_MAX_LENGTH);

            builder.Property(x => x.PaymentMethod)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_METHOD)
                .HasMaxLength(PaymentConst.PAYMENT_METHOD_MAX_LENGTH)
                .IsRequired();

            builder.Property(x => x.PaymentDate)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_DATE)
                .IsRequired();
            builder.Property(x => x.ResponseCode)
               .HasColumnName(PaymentConst.FIELD_PAYMENT_RESPONSE_CODE)
               .IsRequired();

            builder.Property(x => x.PaymentStatus)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_STATUS)
                .HasMaxLength(PaymentConst.PAYMENT_STATUS_MAX_LENGTH)
                .IsRequired();

            builder.Property(x => x.InsertedAt)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_INSERTED_AT);

            builder.Property(x => x.InsertedBy)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_INSERTED_BY);

            builder.Property(x => x.UpdatedAt)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_UPDATED_AT);

            builder.Property(x => x.UpdatedBy)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_UPDATED_BY);

            builder.Property(x => x.IsDeleted)
                .HasColumnName(PaymentConst.FIELD_PAYMENT_IS_DELETED)
                .IsRequired();

            builder.ToTable(PaymentConst.TABLE_PAYMENT);
            //builder.HasMany(c => c.PaymentDetails).WithOne(verify => verify.Payment).HasForeignKey(mer => mer.PaymentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
