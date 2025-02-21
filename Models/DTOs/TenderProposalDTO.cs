using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.DTOs
{

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
