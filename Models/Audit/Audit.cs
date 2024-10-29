using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Audit
{
    public class Audit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        [Required]
        public int AuditorId { get; set; }

        public int? DoctorId { get; set; }

        [Required]
        public AuditType Type { get; set; }

        [Required]
        public AuditStatus Status { get; set; }

        [StringLength(1000)]
        public string Findings { get; set; }

        [ForeignKey("AuditorId")]
        public virtual User Auditor { get; set; }

        [ForeignKey("DoctorId")]
        public virtual User Doctor { get; set; }

        public virtual ICollection<AuditItem> Items { get; set; }
    }

    public enum AuditType
    {
        Regular,
        Controlled,
        Emergency,
        Periodic
    }

    public enum AuditStatus
    {
        Scheduled,
        InProgress,
        Completed,
        RequiresAction
    }

    

}
