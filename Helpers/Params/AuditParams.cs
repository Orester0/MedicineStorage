using MedicineStorage.Models.AuditModels;

namespace MedicineStorage.Helpers.Params
{
    public class AuditParams : Params
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public AuditStatus? Status { get; set; }
        public string? SortyBy { get; set; }
    }
}
