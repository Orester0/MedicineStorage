using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MedicineStorage.Models.DTOs
{
    public class ReturnTenderReportDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TenderStatus Status { get; set; }
        public decimal RequiredQuantity { get; set; }
    }

}
