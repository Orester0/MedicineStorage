using MedicineStorage.Helpers;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnAuditDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AuditStatus Status { get; set; }
        public virtual ReturnUserDTO PlannedByUser { get; set; }
        public virtual ReturnUserDTO? ClosedByUser { get; set; }
        public virtual ICollection<ReturnAuditItemDTO> AuditItems { get; set; }
        public virtual ICollection<ReturnAuditNoteDTO> Notes { get; set; } = new List<ReturnAuditNoteDTO>();
    }

    public class ReturnAuditItemDTO
    {
        public int Id { get; set; }
        public int AuditId { get; set; }
    
        public decimal ExpectedQuantity { get; set; }

        public decimal ActualQuantity { get; set; }

        public virtual ReturnMedicineDTO Medicine { get; set; }
        public virtual User? CheckedByUser { get; set; }
    }

    public class CreateAuditDTO
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

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


    public class CreateAuditNoteDTO
    {

        [StringLength(500)]
        public string? Note { get; set; }

    }

    public class ReturnAuditNoteDTO
    {
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        [MinLength(1)]
        public Dictionary<int, decimal>? ActualQuantities { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }


}
