using Commerce.Command.Domain.Constants.User;
using Commerce.Command.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commerce.Command.Persistence.Configurations.User
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
