﻿using MedicineStorage.Models.MedicineModels;

namespace MedicineStorage.DTOs
{
    public class MedicineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool RequiresSpecialApproval { get; set; }
        public decimal MinimumStock { get; set; }
        public bool RequiresStrictAudit { get; set; }
        public int AuditFrequencyDays { get; set; }
    }

    public class CreateMedicineDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool RequiresSpecialApproval { get; set; }
        public decimal MinimumStock { get; set; }
        public bool RequiresStrictAudit { get; set; }
        public int AuditFrequencyDays { get; set; }
    }

    public class MedicineRequestDTO
    {
        public int RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
    }

    public class MedicineUsageDTO
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int UsedByUserId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime UsageDate { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateMedicineRequestDTO
    {
        public int RequestedByUserId { get; set; }
        public int MedicineId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime RequiredByDate { get; set; }
        public string? Justification { get; set; }
    }

    public class CreateMedicineUsageDTO
    {
        public int RequestedByUserId { get; set; }
        public int MedicineId { get; set; }
        public decimal Quantity { get; set; }
        public string? Notes { get; set; }
    }
}
