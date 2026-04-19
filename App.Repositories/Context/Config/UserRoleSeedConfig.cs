using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Context.Config
{
    public class UserRoleSeedConfig : IEntityTypeConfiguration<IdentityUserRole<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<long>> builder)
        {
            builder.HasData(
                // SuperAdmin has SuperAdmin + Admin roles
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.SuperAdminRoleId,
                    UserId = UserSeedConfig.SuperAdminUserId
                },
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.AdminRoleId,
                    UserId = UserSeedConfig.SuperAdminUserId
                },

                // Admin has Admin role
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.AdminRoleId,
                    UserId = UserSeedConfig.AdminUserId
                },

                // Manager has Manager role
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.ManagerRoleId,
                    UserId = UserSeedConfig.ManagerUserId
                },

                // User has User role
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.UserRoleId,
                    UserId = UserSeedConfig.UserUserId
                },

                // Guest has Guest role
                new IdentityUserRole<long>
                {
                    RoleId = RoleSeedConfig.GuestRoleId,
                    UserId = UserSeedConfig.GuestUserId
                }
            );
        }
    }
}
