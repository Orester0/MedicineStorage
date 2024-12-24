﻿using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public enum TenderItemStatus
    {
        Pending,
        Executed
    }

    public class TenderItem
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int TenderId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }

        [Required]
        public decimal RequiredQuantity { get; set; }

        [Required]
        public TenderItemStatus Status { get; set; } = TenderItemStatus.Pending;
        public virtual Medicine Medicine { get; set; }
    }
}
