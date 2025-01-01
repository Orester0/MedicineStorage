﻿using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public class Tender
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }

        [ForeignKey("OpenedByUser")]
        public int? OpenedByUserId { get; set; }

        [ForeignKey("ClosedByUser")]
        public int? ClosedByUserId { get; set; }

        [ForeignKey("WinnerSelectedByUser")]
        public int? WinnerSelectedByUserId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        [Required]
        public DateTime DeadlineDate { get; set; }

        [Required]
        public TenderStatus Status { get; set; }

        public virtual User CreatedByUser { get; set; }
        public virtual User? OpenedByUser { get; set; }
        public virtual User? ClosedByUser { get; set; }
        public virtual User? WinnerSelectedByUser { get; set; }

        public virtual ICollection<TenderItem> TenderItems { get; set; }
        public virtual ICollection<TenderProposal> TenderProposals { get; set; }
    }

    public enum TenderStatus
    {
        Created,
        Published,
        Closed,
        Awarded,
        Executing,
        Executed,
        Cancelled
    }
}
