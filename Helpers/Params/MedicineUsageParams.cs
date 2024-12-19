namespace MedicineStorage.Helpers.Params
{
    public class MedicineUsageParams : Params
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
