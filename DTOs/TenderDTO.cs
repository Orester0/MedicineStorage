using MedicineStorage.Models;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnTenderDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        [Required]
        [ForeignKey("OpenedByUser")]
        public int OpenedByUserId { get; set; }
        [ForeignKey("ClosedByUser")]
        public int ClosedByUserId { get; set; }
        [ForeignKey("WinnerSelectedByUser")]
        public int WinnerSelectedByUserId { get; set; }


        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }
        [Required]
        public TenderStatus Status { get; set; }


        public virtual User CreatedByUser { get; set; }
        public virtual User OpenedByUser { get; set; }
        public virtual User ClosedByUser { get; set; }
        public virtual User WinnerSelectedByUser { get; set; }


        public virtual ICollection<TenderItem> Items { get; set; }
        public virtual ICollection<TenderProposal> Proposals { get; set; }
    }
    public class CreateTenderDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }
    }

    public class ReturnTenderItemDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Tender")]
        public int TenderId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal RequiredQuantity { get; set; }
        public virtual Tender Tender { get; set; }
        public virtual Medicine Medicine { get; set; }

    }

    public class CreateTenderItemDTO
    {
        [Required]
        public int TenderId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public decimal RequiredQuantity { get; set; }
    }

    public class ReturnTenderProposalDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Tender")]
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
    }
    public class CreateTenderProposalDTO
    {
        [Required]
        public int TenderId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }

    public class ReturnTenderProposalItemDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Proposal")]
        public int TenderProposalId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Quantity { get; set; }

        public decimal TotalItemPrice => UnitPrice * Quantity;
    }


    public class CreateTenderProposalItemDTO
    {
        [Required]
        [ForeignKey("Proposal")]
        public int TenderProposalId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Quantity { get; set; }
    }



}
