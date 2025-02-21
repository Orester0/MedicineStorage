using MedicineStorage.Models.TemplateModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.DTOs
{

    public abstract class TemplateBaseDTO
    {
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 5)]
        public string Name { get; set; } = string.Empty;
        [Range(1, 365)]
        public int RecurrenceInterval { get; set; }
        public DateTime? LastExecutedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class AuditTemplateDTO : TemplateBaseDTO
    {
        public CreateAuditTemplate CreateDTO { get; set; }
    }

    public class TenderTemplateDTO : TemplateBaseDTO
    {
        public CreateTenderTemplate CreateDTO { get; set; }
    }

    public class MedicineRequestTemplateDTO : TemplateBaseDTO
    {
        public CreateMedicineRequestTemplate CreateDTO { get; set; }
    }

}
