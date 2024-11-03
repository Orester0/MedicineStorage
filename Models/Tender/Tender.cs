using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.Models.TenderModels
{
    public class Tender
    {
        [Required]
        [Key]
        public int Id { get; set; }
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
        [Required]
        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; }
        public virtual ICollection<TenderItem> Items { get; set; }
        public virtual ICollection<TenderProposal> Proposals { get; set; }
    }

    public enum TenderStatus
    {
        Draft,
        Published,
        Closed,
        Awarded,
        Cancelled
    }
}
