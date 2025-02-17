using MedicineStorage.Data.Implementations;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.NotificationModels;
using MedicineStorage.Models.TemplateModels;
using MedicineStorage.Patterns;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MedicineStorage.Services.BusinessServices.Interfaces;

public class TemplateExecutionService(IUnitOfWork _unitOfWork, INotificationTextFactory _notificationTextFactory, INotificationService _notificationService) : ITemplateExecutionService
{
    public async Task CheckAndNotifyAsync()
{
    await CheckAndNotifyForTenderTemplates();
    await CheckAndNotifyForMedicineRequestTemplates();
    await CheckAndNotifyForAuditTemplates();
}

private async Task CheckAndNotifyForTenderTemplates()
{
    var templates = await _unitOfWork.TenderTemplateRepository.GetAllActiveAndDueAsync();
        foreach (var template in templates)
        {
            var (title, message) = _notificationTextFactory.GetNotificationText(NotificationType.TemplateExecutionReminder, template.Name);
            var notification = new Notification
            {
                UserId = template.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.SendNotificationAsync(notification);
        }
    }

private async Task CheckAndNotifyForMedicineRequestTemplates()
{
    var templates = await _unitOfWork.MedicineRequestTemplateRepository.GetAllActiveAndDueAsync();
        foreach (var template in templates)
        {
            var (title, message) = _notificationTextFactory.GetNotificationText(NotificationType.TemplateExecutionReminder, template.Name);
            var notification = new Notification
            {
                UserId = template.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.SendNotificationAsync(notification);
        }
    }

private async Task CheckAndNotifyForAuditTemplates()
{
    var templates = await _unitOfWork.AuditTemplateRepository.GetAllActiveAndDueAsync();
        foreach (var template in templates)
        {
            var (title, message) = _notificationTextFactory.GetNotificationText(NotificationType.TemplateExecutionReminder, template.Name);
            var notification = new Notification
            {
                UserId = template.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.SendNotificationAsync(notification);
        }
    }


}
