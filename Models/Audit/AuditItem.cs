using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Audit
{
    public class AuditItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SystemStock { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discrepancy { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [ForeignKey("AuditId")]
        public virtual Audit Audit { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }
    }
}
