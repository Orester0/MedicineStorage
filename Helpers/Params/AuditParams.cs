using MedicineStorage.Models.AuditModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Helpers.Params
{
    public class AuditParams : Params
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public AuditStatus? Status { get; set; }

        [Range(0, int.MaxValue)]
        public int? PlannedByUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int? ExecutedByUserId { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? SortBy { get; set; }

        public bool IsDescending { get; set; }
    }
}
