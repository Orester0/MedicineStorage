using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineRequestDTO
    {
        public int Id { get; set; }
        public int RequestedByUserId { get; set; }
        public int? ApprovedByUserId { get; set; }
        public int MedicineId { get; set; }


        public decimal Quantity { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? ApprovalDate { get; set; }


        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; } = new List<MedicineUsage>();
    }
    public class CreateMedicineRequestDTO
    {
        [Required]
        public int MedicineId { get; set; }
        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }
        [Required]
        public DateTime RequiredByDate { get; set; }
        [MaxLength(1000)]
        public string? Justification { get; set; }
    }

    public class ReturnMedicineUsageDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int UsedByUserId { get; set; }
        public int MedicineRequestId { get; set; }



        public decimal Quantity { get; set; }
        public DateTime UsageDate { get; set; }
        public string? Notes { get; set; }
    }
    public class CreateMedicineUsageDTO
    {
        [Required]
        public int MedicineId { get; set; }
        [Required]
        public int MedicineRequestId { get; set; }
        [Required, Range(0.1, double.MaxValue)]
        public decimal Quantity { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
