using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class ReturnMedicineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool RequiresSpecialApproval { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal Stock { get; set; }
        public bool RequiresStrictAudit { get; set; }
        public int AuditFrequencyDays { get; set; }
    }

    public class CreateMedicineDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool RequiresSpecialApproval { get; set; }

        public decimal MinimumStock { get; set; }

        public bool RequiresStrictAudit { get; set; }

        public int AuditFrequencyDays { get; set; }
    }

}
