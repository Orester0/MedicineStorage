﻿using MedicineStorage.Models.MedicineModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicineStorage.Models.TenderModels
{
    public class TenderProposalItem
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int TenderProposalId { get; set; }
        [Required]
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
