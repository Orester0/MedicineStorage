using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.AuditModels
{
    public class AuditNote
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

    }


}
