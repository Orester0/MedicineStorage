using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MedicineStorage.Models.Medicine;

namespace MedicineStorage.Models.Tender
{
    public class TenderRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TenderId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [ForeignKey("TenderId")]
        public virtual Tender Tender { get; set; }

        [ForeignKey("RequestId")]
        public virtual MedicineRequest Request { get; set; }
    }

}
