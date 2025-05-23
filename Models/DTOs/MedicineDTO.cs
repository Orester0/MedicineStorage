﻿using MedicineStorage.Helpers;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Validators;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.DTOs
{
    public class MedicineAuditAndTenderDto
    {
        public List<ReturnMedicineShortDTO> MedicinesNeedingAudit { get; set; }
        public List<ReturnMedicineShortDTO> MedicinesNeedingTender { get; set; }
    }

    public class ReturnMedicineShortDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal Stock { get; set; }
        public DateTime? LastAuditDate { get; set; }
    }
    public class ReturnMedicineDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool RequiresSpecialApproval { get; set; }

        public decimal MinimumStock { get; set; }

        public decimal Stock { get; set; }
        public int AuditFrequencyDays { get; set; }
        public DateTime? LastAuditDate { get; set; }
    }

    public class CreateMedicineDTO
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        [UniqueMedicineName]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Category { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }


        [Required]
        [Range(1, 365)]
        public int AuditFrequencyDays { get; set; }
    }

    public class UpdateMedicineDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public bool RequiresSpecialApproval { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStock { get; set; }

        [Required]
        [Range(1, 365)]
        public int AuditFrequencyDays { get; set; }
    }

}
