using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models
{
    public class RefreshToken
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public bool IsRevoked { get; set; }
        [Required]
        public virtual User User { get; set; } = null!;
    }
}
