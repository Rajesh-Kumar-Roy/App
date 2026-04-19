using App.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Context.Config
{
    public class RoleSeedConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        // Use constants for IDs (easier to reference)
        public const long SuperAdminRoleId = 1;
        public const long AdminRoleId = 2;
        public const long ManagerRoleId = 3;
        public const long UserRoleId = 4;
        public const long GuestRoleId = 5;

        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Seed roles
            builder.HasData(
                new ApplicationRole
                {
                    Id = SuperAdminRoleId,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    Description = "Super Administrator with unrestricted access",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ConcurrencyStamp = "e7d8c9a1-b2f4-4c3e-a5d6-1234567890ab"
                },
                new ApplicationRole
                {
                    Id = AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role with full access",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ConcurrencyStamp = "f8e9d0b2-c3f5-5d4f-b6e7-2345678901bc"
                },
                new ApplicationRole
                {
                    Id = ManagerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    Description = "Manager role with elevated privileges",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ConcurrencyStamp = "g9f0e1c3-d4f6-6e5g-c7f8-3456789012cd"
                },
                new ApplicationRole
                {
                    Id = UserRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Standard user role",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ConcurrencyStamp = "h0g1f2d4-e5g7-7f6h-d8g9-4567890123de"
                },
                new ApplicationRole
                {
                    Id = GuestRoleId,
                    Name = "Guest",
                    NormalizedName = "GUEST",
                    Description = "Guest user with limited access",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    ConcurrencyStamp = "i1h2g3e5-f6h8-8g7i-e9h0-5678901234ef"
                }
            );
        }
    }
}
