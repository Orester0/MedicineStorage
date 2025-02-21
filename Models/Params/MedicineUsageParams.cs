using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.Params
{
    public class MedicineUsageParams : Params
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Range(1, int.MaxValue)]
        public int? MedicineId { get; set; }

        public int UsedByUserId { get; set; }


        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
