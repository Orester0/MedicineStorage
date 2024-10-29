using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Medicine
{
    public class MedicineUsage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime UsageDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
