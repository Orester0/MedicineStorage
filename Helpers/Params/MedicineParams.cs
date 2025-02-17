using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Helpers.Params
{
    public class MedicineParams : Params
    {
        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Category { get; set; }
        public bool? RequiresSpecialApproval { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MinStock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? MaxStock { get; set; }
        public bool? RequiresStrictAudit { get; set; }

        [StringLength(100)]
        public string? SortBy { get; set; }

        public bool IsDescending { get; set; }
    }
}
