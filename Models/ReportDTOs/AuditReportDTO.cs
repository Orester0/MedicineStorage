using MedicineStorage.Helpers;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnAuditReportDTO
    {
        public string Title { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AuditStatus Status { get; set; }
        public decimal ExpectedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public virtual ReturnUserGeneralDTO? CheckedByUser { get; set; }
    }

}
