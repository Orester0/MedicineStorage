using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.Medicine;

namespace MedicineStorage.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public virtual ICollection<MedicineRequest> Requests { get; set; }
        public virtual ICollection<Audit> ConductedAudits { get; set; }
        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; }
    }

    public enum UserRole
    {
        Doctor,
        Nurse,
        Administrator,
        HeadAdministrator
    }

}
