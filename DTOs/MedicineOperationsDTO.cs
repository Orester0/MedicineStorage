using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineRequestDTO
    {
        [Required]
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
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [EnumDataType(typeof(RequestStatus))]
        public RequestStatus Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime RequiredByDate { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovalDate { get; set; }

        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; } = new List<MedicineUsage>();
    }

    public class CreateMedicineRequestDTO
    {
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime RequiredByDate { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }
    }

    public class ReturnMedicineUsageDTO
    {
        [Required]
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
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime UsageDate { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    public class CreateMedicineUsageDTO
    {
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        [ForeignKey("MedicineRequest")]
        public int MedicineRequestId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
