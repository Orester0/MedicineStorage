using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class MedicineDTO
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

    public class MedicineRequestDTO
    {
        [Required]
        public int RequestedByUserId { get; set; }

        [Required, MaxLength(200)]
        public string RequestedByUserName { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required, MaxLength(200)]
        public string MedicineName { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public DateTime RequiredByDate { get; set; }

        [MaxLength(1000)]
        public string? Justification { get; set; }
    }

    public class MedicineUsageDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required, MaxLength(200)]
        public string MedicineName { get; set; }

        [Required]
        public int UsedByUserId { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime UsageDate { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }

    public class CreateMedicineRequestDTO
    {
        [Required]
        public int RequestedByUserId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        public DateTime RequiredByDate { get; set; }

        [MaxLength(1000)]
        public string? Justification { get; set; }
    }

    public class UpdateRequestStatusDTO
    {
        [Required]
        public RequestStatus NewStatus { get; set; }
        [Required]
        public int UpdatedByUserId { get; set; }
    }

    public class SpecialApprovalDTO
    {
        [Required]
        public bool IsApproved { get; set; }
        [Required]
        public int ApprovedByUserId { get; set; }
    }

    public class CreateMedicineUsageDTO
    {
        [Required]
        public int RequestedByUserId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
