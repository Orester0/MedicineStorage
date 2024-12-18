using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineRequest
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("RequestedByUser")]
        public int RequestedByUserId { get; set; }
        [ForeignKey("ApprovedByUser")]
        public int? ApprovedByUserId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }


        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? ApprovalDate { get; set; }


        public virtual User RequestedByUser { get; set; }
        public virtual User? ApprovedByUser { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; } = new List<MedicineUsage>();
    
    }   


    public enum RequestStatus
    {
        Pending,
        PedingWithSpecial,
        Approved,
        Rejected
    }
}
