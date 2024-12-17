using MedicineStorage.Models.TenderModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class CreateTenderDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DeadlineDate { get; set; }
        [Required]
        public List<CreateTenderItemDTO> Items { get; set; }
    }

    public class ReturnTenderDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }
        public List<ReturnTenderItemDTO> Items { get; set; }
    }

    public class CreateTenderItemDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        public decimal RequiredQuantity { get; set; }
    }

    public class ReturnTenderItemDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public decimal RequiredQuantity { get; set; }
    }

    public class CreateTenderProposalDTO
    {
        [Required]
        public int TenderId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public List<CreateTenderProposalItemDTO> Items { get; set; }
    }

    public class ReturnTenderProposalDTO
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public ProposalStatus Status { get; set; }
        public List<ReturnTenderProposalItemDTO> Items { get; set; }
    }

    public class CreateTenderProposalItemDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }

    public class ReturnTenderProposalItemDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
    }

}
