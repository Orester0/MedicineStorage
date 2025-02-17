using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.DTOs
{
    public class ReturnTenderDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }
        public virtual ReturnUserDTO CreatedByUser { get; set; }
        public virtual ReturnUserDTO? OpenedByUser { get; set; }
        public virtual ReturnUserDTO? ClosedByUser { get; set; }
        public virtual ReturnUserDTO? WinnerSelectedByUser { get; set; }

        public virtual ICollection<ReturnTenderItemDTO> Items { get; set; }
        public virtual ICollection<ReturnTenderProposalDTO> Proposals { get; set; }
    }

    public class ReturnTenderProposalItemDTO
    {
        public int Id { get; set; }
        public int TenderProposalId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalItemPrice => UnitPrice * Quantity;

        public virtual ReturnMedicineDTO Medicine { get; set; }
    }

    public class ReturnTenderProposalDTO
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public ProposalStatus Status { get; set; }
        public virtual ReturnUserDTO CreatedByUser { get; set; }
        public virtual ICollection<ReturnTenderProposalItemDTO> Items { get; set; }
    }

    public class ReturnTenderItemDTO
    {
        public int Id { get; set; }
        public int TenderId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public TenderItemStatus Status { get; set; }
        public virtual ReturnMedicineDTO Medicine { get; set; }

    }

    public class CreateTenderDTO
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; }


        [Required]
        [FutureDateAttribute]
        public DateTime? DeadlineDate { get; set; }
    }

    

    public class CreateTenderItemDTO
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal RequiredQuantity { get; set; }
    }

    
    public class CreateTenderProposalDTO
    {
        [Required]
        public List<CreateTenderProposalItemDTO> ProposalItemsDTOs { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public decimal TotalPrice { get; set; }
    }


    public class CreateTenderProposalItemDTO
    {
        [Required]
        public int TenderProposalId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }
    }



}
