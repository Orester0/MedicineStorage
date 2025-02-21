using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Validators;
using MedicineStorage.Models.UserModels;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineSupply
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedByUserId { get; set; }
        [ForeignKey("Tender")]
        public int? TenderId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }
        [Required]
        [FormerDate]
        public DateTime TransactionDate { get; set; }
        public Medicine Medicine { get; set; }
        public User? CreatedByUser { get; set; }
        public Tender? Tender { get; set; }
    }
}
