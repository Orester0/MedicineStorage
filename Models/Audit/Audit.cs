using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Models.AuditModels
{
    public class Audit
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("ConductedByUser")]
        [Required]
        public int ConductedByUserId { get; set; }
        [Required]
        public DateTime AuditDate { get; set; }
        public string? Notes { get; set; }
        [Required]
        public AuditStatus Status { get; set; }

        public virtual User ConductedByUser { get; set; }
        public virtual ICollection<AuditItem> AuditItems { get; set; }
    }

    public enum AuditStatus
    {
        Planned,
        InProgress,
        Completed,
        RequiresFollowUp
    }
}
