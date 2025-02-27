using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Params
{
    public class MedicineRequestParams : Params
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public List<RequestStatus>? Statuses { get; set; } 

        [Range(1, int.MaxValue)]
        public int? RequestedByUserId { get; set; }

        [Range(1, int.MaxValue)]
        public int? ApprovedByUserId { get; set; }

        [Range(1, int.MaxValue)]
        public int? MedicineId { get; set; }

        [Range(0.1, double.MaxValue)]
        public decimal? MinQuantity { get; set; }

        [Range(0.1, double.MaxValue)]
        public decimal? MaxQuantity { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
