
using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public class TenderProposalItem
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int TenderProposalId { get; set; }
        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
