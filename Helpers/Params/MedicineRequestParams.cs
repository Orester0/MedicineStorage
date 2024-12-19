using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Helpers.Params
{
    public class MedicineRequestParams : Params
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public RequestStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
