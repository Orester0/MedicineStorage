using MedicineStorage.Helpers;
using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    [Index(nameof(Status))]
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
        [Range(0.01, double.MaxValue)]
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
        Submitted = 1,
        Accepted = 2,
        Rejected = 3
    }
}
