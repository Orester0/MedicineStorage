using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public int UserId { get; set; }
    }
}
