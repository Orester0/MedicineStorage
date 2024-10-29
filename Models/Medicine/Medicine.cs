using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Medicine
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentStock { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }

        [Required]
        public bool IsControlled { get; set; }

        [StringLength(500)]
        public string StorageRequirements { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<MedicineRequestItem> RequestItems { get; set; }
        public virtual ICollection<MedicineUsage> UsageRecords { get; set; }
        public virtual ICollection<StockAdjustment> StockAdjustments { get; set; }
    }
    
}
