namespace MedicineStorage.Patterns
{
    public interface INotificationTextFactory
    {
        (string title, string message) GetNotificationText(NotificationType type, params object[] args);
    }
}
