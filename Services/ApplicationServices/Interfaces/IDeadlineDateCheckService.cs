namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface IDeadlineDateCheckService
    {
        Task CheckAndCloseExpiredTendersAsync();
    }
}
