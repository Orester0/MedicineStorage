using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.MedicineModels
{
    public class Stock
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
