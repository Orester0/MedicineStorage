using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.AuditModels
{
    public class AuditItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Audit")]
        public int AuditId { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal ExpectedQuantity { get; set; }
        [Required]
        public decimal ActualQuantity { get; set; }

        public virtual Audit Audit { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
