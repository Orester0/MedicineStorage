using MailKit.Security;
using MimeKit;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isHtml = true);
        public Task SendEmailWithAttachmentAsync(string toEmail, string subject, string message, string attachmentPath);
        public Task SendEmailWithTemplateAsync(string toEmail, string subject, string templateName, Dictionary<string, string> parameters);

        public Task SendEmailWithRetryAsync(string toEmail, string subject, string message, int maxRetries = 3);

    }
}
