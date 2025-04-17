using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnMedicineReportDTO
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool RequiresSpecialApproval { get; set; }

        public decimal MinimumStock { get; set; }

        public decimal Stock { get; set; }
        public int AuditFrequencyDays { get; set; }
        public DateTime? LastAuditDate { get; set; }
    }

}
