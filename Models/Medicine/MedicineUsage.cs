﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.MedicineModels
{
    public class MedicineUsage
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        [ForeignKey("UsedByUser")]
        public int UsedByUserId { get; set; }
        [Required]
        [ForeignKey("Stock")]
        public int StockId { get; set; }

        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public DateTime UsageDate { get; set; }
        public string? Notes { get; set; }
        

        public virtual Medicine Medicine { get; set; }
        public virtual User UsedByUser { get; set; }
    }
}
