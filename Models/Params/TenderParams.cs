using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Params
{
    public class TenderParams : Params
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeadlineDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeadlineDateTo { get; set; }
        public List<TenderStatus>? Statuses { get; set; }

        [Range(0, int.MaxValue)]
        public int? CreatedByUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int? OpenedByUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int? ClosedByUserId { get; set; }

        public List<int>? MedicineIds { get; set; }

        [Range(0, int.MaxValue)]
        public int? WinnerSelectedByUserId { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }

}
