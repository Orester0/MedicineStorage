namespace MedicineStorage.DTOs
{
    public class StartAuditRequest
    {
        public int[] MedicineIds { get; set; }
    }

    public class UpdateAuditItemsRequest
    {
        public Dictionary<int, decimal> ActualQuantities { get; set; }
    }
}
