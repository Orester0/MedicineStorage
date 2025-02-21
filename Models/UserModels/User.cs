using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.UserModels
{
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

        [Column(TypeName = "varbinary(MAX)")]
        public byte[]? Photo { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    }

    
}
