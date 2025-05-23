﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicineStorage.Models.MedicineModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class MedicineCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }

}
