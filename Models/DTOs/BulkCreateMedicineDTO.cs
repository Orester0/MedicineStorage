using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.DTOs
{
    public class BulkCreateMedicineDTO
    {
        public CreateMedicineDTO Medicine { get; set; }

        [Range(1, double.MaxValue)]
        public decimal InitialStock { get; set; }
    }
}
