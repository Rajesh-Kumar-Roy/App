using Microsoft.AspNetCore.Identity;

namespace App.Entities.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<IdentityUserRole<long>> UserRoles { get; set; }
        public virtual ICollection<IdentityUserClaim<long>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<long>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<long>> Tokens { get; set; }
    }
}
