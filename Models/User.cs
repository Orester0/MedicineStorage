using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models
{
    public class User : IdentityUser<int>
    {

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }



        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }

    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }

    public class AppRole : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
