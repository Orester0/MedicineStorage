using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.DTOs;

namespace MedicineStorage.Models.Tender
{

    public class MedicineSupply
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }


        [Required]
        [ForeignKey("TenderProposalItem")]
        public int TenderProposalItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [FormerDate]
        public DateTime TransactionDate { get; set; }


        public Medicine Medicine { get; set; }

        public virtual TenderProposalItem TenderProposalItem { get; set; }
    }
}
