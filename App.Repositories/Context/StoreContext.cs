using App.Entities.Entities;
using App.Entities.Entities.Base;
using App.Entities.Identity;
using App.Repositories.Common.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace App.Repositories.Context
{
    public class StoreContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        private readonly ICurrentUserService _currentUserService;

        public StoreContext(DbContextOptions<StoreContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        // Define DbSet properties for your entities
        public DbSet<Customer> Customers { get; set; }

        // use for migration
        // Add-Migration FirstMigrationStoreContext -Context StoreContext -OutputDir Data/Migrations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This configures Identity automatically with primary keys

            // Custom table name configurations
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<long>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<long>>().ToTable("UserTokens");

            modelBuilder.Entity<ApplicationRole>()
                .Property(r => r.Id)
                .UseIdentityColumn(100, 1); // Start from 100

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Id)
                .UseIdentityColumn(100, 1);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAudit>();
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = currentTime;
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.IsDelete = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = currentTime;
                    entry.Entity.UpdatedBy = currentUserId;
                    entry.Property(nameof(IAudit.CreatedAt)).IsModified = false;
                    entry.Property(nameof(IAudit.CreatedBy)).IsModified = false;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
