using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.AuditModels;
using Microsoft.AspNetCore.Identity;

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

        public virtual ICollection<MedicineRequest> MedicineRequests { get; set; } = [];
        public virtual ICollection<Audit> ConductedAudits { get; set; } = [];
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }

    public class UserRole : IdentityUserRole<int> 
    {
        public User User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }

}
