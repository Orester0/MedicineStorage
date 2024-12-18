using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        [Required, MaxLength(100)]
        public string Category { get; set; }
        [Required]
        public bool RequiresSpecialApproval { get; set; }
        [Required, Range(0.1, double.MaxValue)]
        public decimal MinimumStock { get; set; }
        [Required]
        public bool RequiresStrictAudit { get; set; }
        [Required, Range(1, 365)]
        public int AuditFrequencyDays { get; set; }
    }

    public class CreateMedicineDTO
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal MinimumStock { get; set; }

        [Required]
        public bool RequiresStrictAudit { get; set; }

        [Required, Range(1, 365)]
        public int AuditFrequencyDays { get; set; }
    }

}
