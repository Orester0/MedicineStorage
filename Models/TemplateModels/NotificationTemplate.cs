using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.TemplateModels
{
    public class NotificationTemplate<T>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int RecurrenceInterval { get; set; }
        [Required]
        public DateTime? LastExecutedDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public T? CreateDTO { get; set; }
    }
}
