using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Helpers.Params
{
    public class MedicineUsageParams : Params
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? MedicineId { get; set; }


        [Range(0.1, double.MaxValue)]
        public decimal? MinQuantity { get; set; }

        [Range(0.1, double.MaxValue)]
        public decimal? MaxQuantity { get; set; }


        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
