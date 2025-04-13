using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MedicineStorage.Models.MedicineModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Medicine : ISoftDeletable
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Stock { get; set; }

        [Required]
        [Range(1, 365)]
        public int AuditFrequencyDays { get; set; }

        [AllowNull]
        public DateTime? LastAuditDate { get; set; }



        public virtual MedicineCategory Category { get; set; }

        public bool IsDeleted { get; set; } = false;

    }

}
