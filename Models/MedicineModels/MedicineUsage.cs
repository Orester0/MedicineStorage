using MedicineStorage.DTOs;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineUsage
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }

        [Required]
        [ForeignKey("UsedByUser")]
        public int UsedByUserId { get; set; }

        [Required]
        [ForeignKey("MedicineRequest")]
        public int MedicineRequestId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime UsageDate { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }


        public Medicine Medicine { get; set; }
        public virtual MedicineRequest MedicineRequest { get; set; }
        public virtual User UsedByUser { get; set; }
    }
}
