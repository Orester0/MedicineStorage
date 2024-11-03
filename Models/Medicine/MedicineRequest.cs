using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineRequest
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [ForeignKey("RequestedByUser")]
        [Required]
        public int RequestedByUserId { get; set; }
        [ForeignKey("ApprovedByUser")]
        [Required]
        public int? ApprovedByUserId { get; set; }
        [ForeignKey("Medicine")]
        [Required]
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
    }


    public enum RequestStatus
    {
        Pending,
        ApprovalRequired,
        Approved,
        Rejected,
        Completed
    }
}
