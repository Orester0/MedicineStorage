using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.MedicineModels
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public bool RequiresSpecialApproval { get; set; }


        [Required]
        public decimal MinimumStock { get; set; }
        [Required]
        public bool RequiresStrictAudit { get; set; }
        [Required]
        public int AuditFrequencyDays { get; set; }

        public virtual ICollection<Stock> StockRecords { get; set; }
        public virtual ICollection<MedicineRequest> Requests { get; set; }
        public virtual ICollection<MedicineUsage> UsageRecords { get; set; }
    }

}
