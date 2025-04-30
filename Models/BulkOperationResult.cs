namespace MedicineStorage.Models
{
    public class BulkOperationResult
    {
        public int SuccessCount { get; set; } = 0;
        public Dictionary<string, string> Failures { get; } = new Dictionary<string, string>();

        public void AddFailure(string itemName, string reason)
        {
            Failures.Add(itemName, reason);
        }

        public int FailureCount => Failures.Count;
    }
}
