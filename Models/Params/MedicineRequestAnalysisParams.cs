using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Params
{
    public class MedicineRequestAnalysisParams : Params
    {
        [Range(1, int.MaxValue)]
        public int? MedicineId { get; set; }

        public List<RequestStatus>? Statuses { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [StringLength(100)]
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
