using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Patterns;

namespace MedicineStorage.Patterns
{
    public interface INotificationTextStrategy
    {
        string GenerateTitle();
        string GenerateMessage(params object[] args);
    }

    public class MedicineRequestApprovedNotificationStrategy : INotificationTextStrategy
    {
        public string GenerateTitle() => "Medicine Request Approved";
        public string GenerateMessage(params object[] args) => $"Your medicine request for {args[0]} has been approved.";
    }

    public class MedicineRequestRejectedNotificationStrategy : INotificationTextStrategy
    {
        public string GenerateTitle() => "Medicine Request Rejected";
        public string GenerateMessage(params object[] args) => $"Your medicine request for {args[0]} has been rejected.";
    }

    public class TenderProposalWonNotificationStrategy : INotificationTextStrategy
    {
        public string GenerateTitle() => "Tender Proposal Won";
        public string GenerateMessage(params object[] args) => $"Your proposal for tender {args[0]} has been selected as the winning proposal.";
    }

    public class TenderClosedNotificationStrategy : INotificationTextStrategy
    {
        public string GenerateTitle() => "Tender Closed";
        public string GenerateMessage(params object[] args) => $"The tender {args[0]} that you applied for has been closed.";
    }

    public class TemplateExecutionReminderNotificationStrategy : INotificationTextStrategy
    {
        public string GenerateTitle() => "Template Execution Reminder";
        public string GenerateMessage(params object[] args) => $"The template {args[0]} needs execution.";
    }

    public enum NotificationType
    {
        MedicineRequestApproved,
        MedicineRequestRejected,
        TenderProposalWon,
        TenderClosed,
        TemplateExecutionReminder
    }

    
}
