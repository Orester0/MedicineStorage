using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Models.DTOs
{
    public class MedicineStockForecastDTO
    {
        public ReturnMedicineDTO Medicine { get; set; }
        public int CurrentStock { get; set; }
        public int? TenderStock { get; set; }
        public int? RequestedAmount { get; set; }
        public int ProjectedStock { get; set; }
        public bool NeedsRestock { get; set; }
    }
}
