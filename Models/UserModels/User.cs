using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.UserModels
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

    
}
