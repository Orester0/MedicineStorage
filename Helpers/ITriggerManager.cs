namespace MedicineStorage.Helpers
{
    public interface ITriggerManager
    {
        Task DisableTriggersAsync();
        Task EnableTriggersAsync();
    }
}
