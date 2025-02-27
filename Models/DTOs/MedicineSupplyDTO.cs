namespace MedicineStorage.Models.DTOs
{
    public class ReturnMedicineSupplyDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public ReturnMedicineDTO Medicine { get; set; }
        public ReturnUserGeneralDTO? CreatedByUser { get; set; }
        public ReturnTenderDTO? Tender { get; set; }
    }

    public class CreateMedicineSupplyDTO
    {
        public int MedicineId { get; set; }
        public decimal Quantity { get; set; }
    }
}

