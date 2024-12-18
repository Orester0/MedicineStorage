namespace MedicineStorage.DTOs
{
    public class CreateAuditRequest
    {
        public int[] MedicineIds { get; set; }
        public string? Notes { get; set; }
        public DateTime PlannedDate { get; set; }
    }

    public class StartAuditRequest
    {
        public int AuditId { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        public int AuditId { get; set; }
        public Dictionary<int, decimal> ActualQuantities { get; set; }
        public string? Notes { get; set; }
    }

    public class CloseAuditRequest
    {
        public int AuditId { get; set; }
        public string? Notes { get; set; }
    }
}
