using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnMedicineUsageDTO
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public DateTime UsageDate { get; set; }
        public ReturnMedicineDTO Medicine { get; set; }
        public virtual ReturnUserGeneralDTO UsedByUser { get; set; }
    }



    public class CreateMedicineUsageDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
    }
}
