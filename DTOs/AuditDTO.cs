using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class CreateAuditRequest
    {
        [Required]
        public int[] MedicineIds { get; set; }

        [Required]
        public string? Notes { get; set; }

        [Required]
        public DateTime PlannedDate { get; set; }
    }

    public class AuditNotes
    {
        [Required]
        public string? Notes { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        [Required]
        public Dictionary<int, decimal> ActualQuantities { get; set; }

        [Required]
        public string? Notes { get; set; }
    }

}
