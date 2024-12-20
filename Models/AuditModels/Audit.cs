using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.AuditModels
{
    public class Audit
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PlannedByUser")]
        public int PlannedByUserId { get; set; }

        [ForeignKey("ExecutedByUser")]
        public int ExecutedByUserId { get; set; }
        [Required]
        public DateTime PlannedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        [Required]
        public AuditStatus Status { get; set; }


        public virtual User PlannedByUser { get; set; }
        public virtual User ExecutedByUser { get; set; }
        public virtual ICollection<AuditItem> AuditItems { get; set; }
    }

    public enum AuditStatus
    {
        Planned,
        InProgress,
        Completed,
        RequiresFollowUp,
        Cancelled
    }


}
