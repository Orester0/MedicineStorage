namespace MedicineStorage.Helpers
{
    public interface IStoredProceduresGenerator
    {

        Task CreateGetOrInsertCategoryProcedureAsync();
        Task CreateCleanupUnusedCategoryTriggerAsync();
        Task CreateUpdateMinimumStockProcedureAsync();
        Task CreateCheckMedicineRequestApprovalTriggerAsync();
        Task CreateUpdateExpiredTendersProcedureAsync();
        Task CreateUpdateExpiredMedicineRequestsProcedureAsync();
        Task CreateDailyJobForExpiredUpdatesAsync();
    }


}
