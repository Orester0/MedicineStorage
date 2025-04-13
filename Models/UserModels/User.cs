using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.UserModels
{

    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(FirstName), nameof(LastName))]
    [Index(nameof(Company))]
    public class User : IdentityUser<int>
    {

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string? Position { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string? Company { get; set; }

        [StringLength(500)]
        public string? PhotoBlobName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    }

    
}
