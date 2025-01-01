using MedicineStorage.Helpers;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnAuditDTO
    {
        public int Id { get; set; }

        public DateTime PlannedDate { get; set; }

        public DateTime? StartDate { get; set; }
         
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }

        public AuditStatus Status { get; set; }

        public virtual UserDTO PlannedByUser { get; set; }

        public virtual UserDTO? ExecutedByUser { get; set; }

        public virtual ICollection<ReturnAuditItemDTO> AuditItems { get; set; }
    }

    public class ReturnAuditItemDTO
    {
        public int Id { get; set; }

        public int AuditId { get; set; }
        
        public decimal ExpectedQuantity { get; set; }

        public decimal ActualQuantity { get; set; }

        public virtual ReturnMedicineDTO Medicine { get; set; }
    }









    public class CreateAuditRequest
    {
        [Required]
        [MinLength(1)]
        public int[] MedicineIds { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime PlannedDate { get; set; }
    }

    public class AuditNotes
    {
        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        [Required]
        [MinLength(1)]
        public Dictionary<int, decimal> ActualQuantities { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }


}
