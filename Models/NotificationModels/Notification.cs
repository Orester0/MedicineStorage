using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.NotificationModels
{
    [Index(nameof(Title))]
    public class Notification
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 5)]
        public string? Title { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Message { get; set; }
        [Required]
        public bool IsRead { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey("MedicineRequest")]
        public int? MedicineRequestId { get; set; }
        public virtual MedicineRequest MedicineRequest { get; set; }


        [ForeignKey("Tender")]
        public int? TenderId { get; set; }
        public virtual Tender Tender { get; set; }

    }
}
