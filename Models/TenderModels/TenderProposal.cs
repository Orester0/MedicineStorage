using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public class TenderProposal
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int TenderId { get; set; }
        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public DateTime SubmissionDate { get; set; }
        [Required]
        public ProposalStatus Status { get; set; }


        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<TenderProposalItem> Items { get; set; }
    }



    public enum ProposalStatus
    {
        Submitted,
        Accepted,
        Rejected
    }
}
