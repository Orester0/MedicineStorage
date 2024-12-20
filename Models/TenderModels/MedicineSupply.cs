using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Tender
{
    public class MedicineSupply
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("TenderProposalItem")]
        public int TenderProposalItemId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        public virtual TenderProposalItem TenderProposalItem { get; set; }
    }
}
