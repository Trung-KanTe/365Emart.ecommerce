using Commerce.Query.Domain.Constants.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = Commerce.Query.Domain.Entities.User;

namespace Commerce.Query.Persistence.Configurations.User
{
    /// <summary>
    /// EF Core configuration for User entity
    /// </summary>
    public class UserConfig : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(UserConst.FIELD_USER_ID);
            builder.Property(x => x.Name).HasColumnName(UserConst.FIELD_USER_NAME);
            builder.Property(x => x.Email).HasColumnName(UserConst.FIELD_USER_EMAIL);
            builder.Property(x => x.PasswordHash).HasColumnName(UserConst.FIELD_USER_PASSWORD_HASH);
            builder.Property(x => x.Tel).HasColumnName(UserConst.FIELD_USER_TEL);
            builder.Property(x => x.Address).HasColumnName(UserConst.FIELD_USER_ADDRESS);
            builder.Property(x => x.WardId).HasColumnName(UserConst.FIELD_USER_WARD_ID);
            builder.Property(x => x.InsertedBy).HasColumnName(UserConst.FIELD_USER_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(UserConst.FIELD_USER_INSERTED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(UserConst.FIELD_USER_UPDATED_BY);
            builder.Property(x => x.UpdatedAt).HasColumnName(UserConst.FIELD_USER_UPDATED_AT);
            builder.Property(x => x.IsDeleted).HasColumnName(UserConst.FIELD_USER_IS_DELETED);
            builder.ToTable(UserConst.TABLE_USER);
            builder.HasMany(c => c.UserRoles).WithOne(cc => cc.User).HasForeignKey(cc => cc.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
