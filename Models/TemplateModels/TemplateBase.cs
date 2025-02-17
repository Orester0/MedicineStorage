using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MedicineStorage.Models.TemplateModels
{
    public class TemplateBase<T>
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]

        [StringLength(200, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [Range(1, 365)]
        public int RecurrenceInterval { get; set; }
        [AllowNull]
        public DateTime? LastExecutedDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string CreateDTOJson { get; set; }

        [NotMapped]
        public T CreateDTO { get; set; }
    }
}
