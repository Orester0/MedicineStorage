using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnTenderDTO
    {
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }
        public int OpenedByUserId { get; set; }
        public int ClosedByUserId { get; set; }
        public int WinnerSelectedByUserId { get; set; }


        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }


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
        public int Id { get; set; }
        public int TenderId { get; set; }
        public int MedicineId { get; set; }
        public decimal RequiredQuantity { get; set; }

    }

    public class CreateTenderItemDTO
    {
        [Required]
        public int TenderId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public decimal RequiredQuantity { get; set; }
    }

    public class ReturnTenderProposalDTO
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public int CreatedByUserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
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
        public int Id { get; set; }
        public int TenderProposalId { get; set; }
        public int MedicineId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalItemPrice => UnitPrice * Quantity;
    }


    public class CreateTenderProposalItemDTO
    {
        [Required]
        public int TenderProposalId { get; set; }
        [Required]
        public int MedicineId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Quantity { get; set; }
    }



}
