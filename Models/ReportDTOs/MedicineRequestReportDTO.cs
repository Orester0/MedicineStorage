using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnMedicineRequestReportDTO
    {
        public decimal Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public virtual ReturnUserGeneralDTO RequestedByUser { get; set; }
        public virtual ReturnUserGeneralDTO? ApprovedByUser { get; set; }
    }
}
