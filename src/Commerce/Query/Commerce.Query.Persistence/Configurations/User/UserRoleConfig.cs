using Commerce.Query.Domain.Constants.User;
using Commerce.Query.Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Query.Persistence.Configurations.User
{
    /// <summary>
    /// EF Core configuration for UserRole entity
    /// </summary>
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });
            builder.Property(x => x.UserId).HasColumnName(UserConst.FIELD_USER_ROLE_USER_ID);
            builder.Property(x => x.RoleId).HasColumnName(UserConst.FIELD_USER_ROLE_ROLE_ID);
            builder.ToTable(UserConst.TABLE_USER_ROLE);
            builder.HasOne(cc => cc.User).WithMany(c => c.UserRoles).HasForeignKey(cc => cc.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
