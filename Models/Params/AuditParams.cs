﻿using MedicineStorage.Models.AuditModels;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.Params
{
    public class AuditParams : Params
    {
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromPlannedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToPlannedDate { get; set; }
        public List<AuditStatus>? Statuses { get; set; }

        [Range(0, int.MaxValue)]
        public int? PlannedByUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int? ClosedByUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int? ExecutedByUserId { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }

}
