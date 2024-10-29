using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Medicine
{
    public class StockAdjustment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime AdjustmentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        public AdjustmentType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    public enum AdjustmentType
    {
        Receipt,
        Disposal,
        Loss,
        Correction,
        Return
    }
}
