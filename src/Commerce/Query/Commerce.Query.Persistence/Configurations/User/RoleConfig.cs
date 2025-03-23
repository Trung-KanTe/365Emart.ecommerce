using Commerce.Query.Domain.Constants.User;
using Commerce.Query.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Query.Persistence.Configurations.User
{
    /// <summary>
    /// EF Core configuration for Role entity
    /// </summary>
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(UserConst.FIELD_ROLE_ID);
            builder.Property(x => x.Name).HasColumnName(UserConst.FIELD_ROLE_NAME);
            builder.Property(x => x.Description).HasColumnName(UserConst.FIELD_ROLE_DESCRIPTION);
            builder.Property(x => x.InsertedBy).HasColumnName(UserConst.FIELD_ROLE_INSERTED_BY);
            builder.Property(x => x.InsertedAt).HasColumnName(UserConst.FIELD_ROLE_INSERTED_AT);
            builder.Property(x => x.UpdatedBy).HasColumnName(UserConst.FIELD_ROLE_UPDATED_BY);
            builder.Property(x => x.UpdatedAt).HasColumnName(UserConst.FIELD_ROLE_UPDATED_AT);
            builder.Property(x => x.IsDeleted).HasColumnName(UserConst.FIELD_ROLE_IS_DELETED);
            builder.ToTable(UserConst.TABLE_ROLE);
        }
    }
}
