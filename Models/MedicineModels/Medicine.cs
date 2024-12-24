using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.MedicineModels
{
    public class Medicine
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Stock { get; set; }

        [Required]
        public bool RequiresStrictAudit { get; set; }

        [Required]
        [Range(1, 365)]
        public int AuditFrequencyDays { get; set; }

        public virtual ICollection<MedicineRequest> Requests { get; set; }
        public virtual ICollection<MedicineUsage> UsageRecords { get; set; }
    }

}
