using MedicineStorage.Models.TenderModels;

namespace MedicineStorage.Helpers.Params
{
    public class TenderParams : Params
    {
        public string? Title { get; set; }
        public DateTime? PublishDateFrom { get; set; }
        public DateTime? PublishDateTo { get; set; }
        public TenderStatus? Status { get; set; }
        public string? SortBy { get; set; }
    }

}
