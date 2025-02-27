using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.DTOs
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
        public virtual ReturnUserGeneralDTO RequestedByUser { get; set; }
        public virtual ReturnUserGeneralDTO? ApprovedByUser { get; set; }
    }

    public class CreateMedicineRequestDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime RequiredByDate { get; set; }

        [StringLength(500, MinimumLength = 5)]
        public string? Justification { get; set; }
    }



}
