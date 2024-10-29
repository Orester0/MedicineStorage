using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Medicine
{
    public class MedicineRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequesterId { get; set; }

        [ForeignKey("RequesterId")]
        public virtual User Requester { get; set; }

        public int? ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public virtual User Approver { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public DateTime RequiredForDate { get; set; }

        [Required]
        public bool IsRecurring { get; set; }

        public int? RecurringIntervalDays { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        public RequestPriority Priority { get; set; }

        public virtual ICollection<MedicineRequestItem> Items { get; set; }
        public virtual ICollection<RequestApproval> Approvals { get; set; }
    }

    public enum RequestStatus
    {
        Draft,
        Pending,
        PendingSpecialApproval,
        Approved,
        Rejected,
        Cancelled,
        Completed
    }

    public enum RequestPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }

    public class MedicineRequestItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [StringLength(500)]
        public string Justification { get; set; }

        [ForeignKey("RequestId")]
        public virtual MedicineRequest Request { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }
    }

    public class RequestApproval
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int ApproverId { get; set; }

        [Required]
        public DateTime ApprovalDate { get; set; }

        [Required]
        public ApprovalStatus Status { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        [ForeignKey("RequestId")]
        public virtual MedicineRequest Request { get; set; }

        [ForeignKey("ApproverId")]
        public virtual User Approver { get; set; }
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
