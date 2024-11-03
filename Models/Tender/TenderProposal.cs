using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Models.TenderModels
{
    public class TenderProposal
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Tender")]
        public int TenderId { get; set; }
        [Required]
        [ForeignKey("Distributor")]
        public int DistributorId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTime SubmissionDate { get; set; }
        [Required]
        public ProposalStatus Status { get; set; }

        // Navigation properties
        public virtual Tender Tender { get; set; }
        public virtual User Distributor { get; set; }
        public virtual ICollection<TenderProposalItem> Items { get; set; }
    }

 

    public enum ProposalStatus
    {
        Submitted,
        UnderReview,
        Accepted,
        Rejected
    }
}
