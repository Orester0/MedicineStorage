﻿using MedicineStorage.Helpers;
using MedicineStorage.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineRequest
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("RequestedByUser")]
        public int RequestedByUserId { get; set; }

        [ForeignKey("ApprovedByUser")]
        public int? ApprovedByUserId { get; set; }

        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [EnumDataType(typeof(RequestStatus))]
        public RequestStatus Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime RequiredByDate { get; set; }

        [StringLength(1000)]
        public string? Justification { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ApprovalDate { get; set; }

        public virtual User RequestedByUser { get; set; }
        public virtual User? ApprovedByUser { get; set; }
        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; } = new List<MedicineUsage>();
    }


    public enum RequestStatus
    {
        Pending,
        PedingWithSpecial,
        Approved,
        Rejected
    }
}