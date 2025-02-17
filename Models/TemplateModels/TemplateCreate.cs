using MedicineStorage.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.TemplateModels
{
    public class CreateAuditTemplate
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [MinLength(1)]
        public int[] MedicineIds { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class CreateMedicineRequestTemplate
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }
    }

    public class CreateTenderTemplate
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string Description { get; set; }
    }



}
