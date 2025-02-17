using MedicineStorage.Patterns;
using MedicineStorage.Services.ApplicationServices.Interfaces;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class NotificationTextFactory : INotificationTextFactory
    {
        private readonly Dictionary<NotificationType, INotificationTextStrategy> _strategies;

        public NotificationTextFactory()
        {
            _strategies = new Dictionary<NotificationType, INotificationTextStrategy>
            {
                { NotificationType.MedicineRequestApproved, new MedicineRequestApprovedNotificationStrategy() },
                { NotificationType.MedicineRequestRejected, new MedicineRequestRejectedNotificationStrategy() },
                { NotificationType.TenderProposalWon, new TenderProposalWonNotificationStrategy() },
                { NotificationType.TenderClosed, new TenderClosedNotificationStrategy() },
                { NotificationType.TemplateExecutionReminder, new TemplateExecutionReminderNotificationStrategy() }
            };
        }

        public (string title, string message) GetNotificationText(NotificationType type, params object[] args)
        {
            if (_strategies.TryGetValue(type, out var strategy))
            {
                return (strategy.GenerateTitle(), strategy.GenerateMessage(args));
            }
            return ("New Notification", "Default notification message.");
        }
    }
}

