using MedicineStorage.Models.UserModels;
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
        public DateTime ClosingDate { get; set; }

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
