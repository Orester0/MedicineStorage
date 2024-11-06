using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public class TenderItem
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Tender")]
        public int TenderId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        public decimal RequiredQuantity { get; set; }

        public virtual Tender Tender { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
