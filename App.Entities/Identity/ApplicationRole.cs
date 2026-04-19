using Microsoft.AspNetCore.Identity;

namespace App.Entities.Identity
{
    public class ApplicationRole : IdentityRole<long>
    {
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<IdentityUserRole<long>> UserRoles { get; set; }
        public virtual ICollection<IdentityRoleClaim<long>> RoleClaims { get; set; }
    }
}
