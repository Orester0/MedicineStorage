using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.Distributor;

namespace MedicineStorage.Models.Tender
{
    public class Tender
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public TenderStatus Status { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? WinningBidId { get; set; }

        [ForeignKey("WinningBidId")]
        public virtual DistributorBid WinningBid { get; set; }

        public virtual ICollection<TenderRequest> Requests { get; set; }
        public virtual ICollection<DistributorBid> Bids { get; set; }
    }

    public enum TenderStatus
    {
        Draft,
        Open,
        Closed,
        Cancelled,
        Completed
    }

}
