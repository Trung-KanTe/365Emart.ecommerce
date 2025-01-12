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
            builder.ToTable(UserConst.TABLE_ROLE);
        }
    }
}
