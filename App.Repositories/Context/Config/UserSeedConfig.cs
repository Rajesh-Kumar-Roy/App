using App.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Repositories.Context.Config
{
    public class UserSeedConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        // Use constants for IDs
        public const long SuperAdminUserId = 1;
        public const long AdminUserId = 2;
        public const long ManagerUserId = 3;
        public const long UserUserId = 4;
        public const long GuestUserId = 5;

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            // SuperAdmin User
            var superAdmin = new ApplicationUser
            {
                Id = SuperAdminUserId,
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@yourproject.com",
                NormalizedEmail = "SUPERADMIN@YOURPROJECT.COM",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "SUPERADMIN-SECURITY-STAMP",
                ConcurrencyStamp = "SUPERADMIN-CONCURRENCY-STAMP",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };
            superAdmin.PasswordHash = hasher.HashPassword(superAdmin, "SuperAdmin@123");

            // Admin User
            var admin = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@yourproject.com",
                NormalizedEmail = "ADMIN@YOURPROJECT.COM",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "ADMIN-SECURITY-STAMP",
                ConcurrencyStamp = "ADMIN-CONCURRENCY-STAMP",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            // Manager User
            var manager = new ApplicationUser
            {
                Id = ManagerUserId,
                UserName = "manager",
                NormalizedUserName = "MANAGER",
                Email = "manager@yourproject.com",
                NormalizedEmail = "MANAGER@YOURPROJECT.COM",
                FirstName = "John",
                LastName = "Manager",
                PhoneNumber = "+8801712345678",
                DateOfBirth = new DateTime(1985, 5, 15),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "MANAGER-SECURITY-STAMP",
                ConcurrencyStamp = "MANAGER-CONCURRENCY-STAMP",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };
            manager.PasswordHash = hasher.HashPassword(manager, "Manager@123");

            // Regular User
            var user = new ApplicationUser
            {
                Id = UserUserId,
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@yourproject.com",
                NormalizedEmail = "USER@YOURPROJECT.COM",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "+8801787654321",
                DateOfBirth = new DateTime(1990, 8, 20),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "USER-SECURITY-STAMP",
                ConcurrencyStamp = "USER-CONCURRENCY-STAMP",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };
            user.PasswordHash = hasher.HashPassword(user, "User@123");

            // Guest User
            var guest = new ApplicationUser
            {
                Id = GuestUserId,
                UserName = "guest",
                NormalizedUserName = "GUEST",
                Email = "guest@yourproject.com",
                NormalizedEmail = "GUEST@YOURPROJECT.COM",
                FirstName = "Guest",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                SecurityStamp = "GUEST-SECURITY-STAMP",
                ConcurrencyStamp = "GUEST-CONCURRENCY-STAMP",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            };
            guest.PasswordHash = hasher.HashPassword(guest, "Guest@123");

            builder.HasData(superAdmin, admin, manager, user, guest);
        }
    }
}
