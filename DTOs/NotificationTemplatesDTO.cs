using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{

    public abstract class NotificationTemplateBaseDTO
    {
        public string Name { get; set; } = string.Empty;
        public int RecurrenceInterval { get; set; }
        public DateTime? LastExecutedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class AuditTemplateDTO : NotificationTemplateBaseDTO
    {
        public CreateAuditDTO CreateDTO { get; set; }
    }

    public class TenderTemplateDTO : NotificationTemplateBaseDTO
    {
        public CreateTenderDTO CreateDTO { get; set; }
    }

    public class MedicineRequestTemplateDTO : NotificationTemplateBaseDTO
    {
        public CreateMedicineRequestDTO CreateDTO { get; set; }
    }

}
