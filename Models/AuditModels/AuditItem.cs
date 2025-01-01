﻿using MedicineStorage.DTOs;
using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.AuditModels
{
    public class AuditItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuditId { get; set; }

        [Required]
        [ForeignKey("ReturnMedicineDTO")]
        public int MedicineId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal ExpectedQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal ActualQuantity { get; set; }

        public virtual Medicine Medicine { get; set; }
    }
}
