using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Validators;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnMedicineSupplyDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public ReturnMedicineDTO Medicine { get; set; }
        public ReturnUserDTO? CreatedByUser { get; set; }
        public ReturnTenderDTO? Tender { get; set; }
    }

    public class CreateMedicineSupplyDTO
    {
        public int MedicineId { get; set; }
        public decimal Quantity { get; set; }
    }
}

