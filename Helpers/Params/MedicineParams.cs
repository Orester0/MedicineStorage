namespace MedicineStorage.Helpers.Params
{
    public class MedicineParams : Params
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool? RequiresSpecialApproval { get; set; }
        public decimal? MinStock { get; set; }
        public decimal? MaxStock { get; set; }
        public decimal? MinMinimumStock { get; set; }
        public decimal? MaxMinimumStock { get; set; }
        public bool? RequiresStrictAudit { get; set; }
        public int? MinAuditFrequencyDays { get; set; }
        public int? MaxAuditFrequencyDays { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
