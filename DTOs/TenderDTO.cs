using MedicineStorage.Models;
using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class CreateTenderDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }
        public int CreatedByUserId { get; set; }
        public virtual ICollection<TenderItem> Items { get; set; }
    }

    public class ReturnTenderDTO
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }
        public int CreatedByUserId { get; set; }

        public virtual ICollection<TenderItem> Items { get; set; }
        public virtual ICollection<TenderProposal> Proposals { get; set; }
    }

    public class TenderItemDto
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public int MedicineId { get; set; }
        public decimal RequiredQuantity { get; set; }
    }

    public class TenderProposalDTO
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public int DistributorId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public ProposalStatus Status { get; set; }
        public ICollection<TenderProposalItemDTO> Items { get; set; }
    }

    public class TenderProposalItemDTO
    {
        public int Id { get; set; }
        public int TenderProposalId { get; set; }
        public int MedicineId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }
}
