using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineRequestDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public ReturnMedicineDTO Medicine { get; set; }
        public virtual UserDTO RequestedByUser { get; set; }
        public virtual UserDTO? ApprovedByUser { get; set; }
    }
    public class ReturnMedicineUsageDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime UsageDate { get; set; }
        public string? Notes { get; set; }
        public ReturnMedicineDTO Medicine { get; set; }
        public virtual ReturnMedicineRequestDTO MedicineRequest { get; set; }
        public virtual UserDTO UsedByUser { get; set; }
    }

    [Keyless]
    [NotMapped]
    public class CreateMedicineRequestDTO
    {
        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDateAttribute]
        public DateTime RequiredByDate { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }
    }

    

    public class CreateMedicineUsageDTO
    {
        [Required]
        [ForeignKey("ReturnMedicineDTO")]
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
