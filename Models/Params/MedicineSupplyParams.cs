using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Params
{
    public class MedicineSupplyParams : Params
    {
        public int? MedicineId { get; set; }
        public int? TenderId { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
