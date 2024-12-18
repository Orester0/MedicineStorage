using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineRequestDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("RequestedByUser")]
        public int RequestedByUserId { get; set; }
        [ForeignKey("ApprovedByUser")]
        public int? ApprovedByUserId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }


        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? ApprovalDate { get; set; }


        public virtual User RequestedByUser { get; set; }
        public virtual User? ApprovedByUser { get; set; }
        public virtual Medicine Medicine { get; set; }
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
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        [ForeignKey("UsedByUser")]
        public int UsedByUserId { get; set; }
        [Required]
        [ForeignKey("MedicineRequest")]
        public int MedicineRequestId { get; set; }


        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public DateTime UsageDate { get; set; }
        public string? Notes { get; set; }


        public virtual MedicineRequest MedicineRequest { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual User UsedByUser { get; set; }
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
