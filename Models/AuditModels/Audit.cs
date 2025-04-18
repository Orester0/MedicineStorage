using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.AuditModels
{

    [Index(nameof(Title))]
    [Index(nameof(Status), nameof(PlannedDate))]
    public class Audit
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [ForeignKey("PlannedByUser")]
        public int PlannedByUserId { get; set; }

        [ForeignKey("ClosedByUser")]
        public int? ClosedByUserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PlannedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [EnumDataType(typeof(AuditStatus))]
        public AuditStatus Status { get; set; }

        public virtual User PlannedByUser { get; set; }
        public virtual User? ClosedByUser { get; set; }
        public virtual ICollection<AuditItem> AuditItems { get; set; }

        public virtual ICollection<AuditNote> Notes { get; set; } = new List<AuditNote>();

    }

    public enum AuditStatus
    {
        Planned = 1,
        InProgress = 2,
        SuccesfullyCompleted = 3,
        CompletedWithProblems = 4,
        Cancelled = 5
    }


}
