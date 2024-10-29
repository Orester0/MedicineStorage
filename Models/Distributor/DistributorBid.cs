using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Distributor
{
    public class DistributorBid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DistributorId { get; set; }

        [Required]
        public int TenderId { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public BidStatus Status { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [ForeignKey("DistributorId")]
        public virtual Distributor Distributor { get; set; }

        [ForeignKey("TenderId")]
        public virtual Tender Tender { get; set; }

        public virtual ICollection<BidItem> Items { get; set; }
    }

    public enum BidStatus
    {
        Draft,
        Submitted,
        Accepted,
        Rejected,
        Cancelled
    }

    public class BidItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BidId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("BidId")]
        public virtual DistributorBid Bid { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }
    }
}
