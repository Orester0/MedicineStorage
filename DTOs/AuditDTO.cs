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
        public string? Notes { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        public Dictionary<int, decimal> ActualQuantities { get; set; }
        public string? Notes { get; set; }
    }

    public class CloseAuditRequest
    {
        public string? Notes { get; set; }
    }
}
