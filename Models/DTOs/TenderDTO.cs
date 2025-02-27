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
    public class ReturnTenderDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public TenderStatus Status { get; set; }
        public virtual ReturnUserGeneralDTO CreatedByUser { get; set; }
        public virtual ReturnUserGeneralDTO? OpenedByUser { get; set; }
        public virtual ReturnUserGeneralDTO? ClosedByUser { get; set; }
        public virtual ReturnUserGeneralDTO? WinnerSelectedByUser { get; set; }

        public virtual ICollection<ReturnTenderItemDTO> Items { get; set; }
        public virtual ICollection<ReturnTenderProposalDTO> Proposals { get; set; }
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
        [FutureDate]
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




}
