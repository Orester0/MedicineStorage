using MailKit.Security;
using MedicineStorage.Services.ApplicationServices.Interfaces;
using MimeKit;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class EmailService(IConfiguration _configuration, ILogger<EmailService> _logger) : IEmailService
    {
        private async Task<bool> IsSmtpServerAvailableAsync()
        {
            try
            {
                using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                await smtpClient.ConnectAsync(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                await smtpClient.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP server is not available.");
                return false;
            }
        }


        public async Task SendEmailAsync(string toEmail, string subject, string message, bool isHtml = true)
        {
            try
            {
                if (!await IsSmtpServerAvailableAsync())
                {
                    _logger.LogError("SMTP server is unavailable. Email cannot be sent.");
                    throw new InvalidOperationException("SMTP server is unavailable.");
                }

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromEmail"]));
                emailMessage.To.Add(MailboxAddress.Parse(toEmail));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder();

                if (isHtml)
                {
                    bodyBuilder.HtmlBody = message;
                }
                else
                {
                    bodyBuilder.TextBody = message;
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                await smtpClient.ConnectAsync(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]);
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
                throw;
            }
        }
        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string message, string attachmentPath)
        {
            try
            {
                if (!await IsSmtpServerAvailableAsync())
                {
                    _logger.LogError("SMTP server is unavailable. Email cannot be sent.");
                    throw new InvalidOperationException("SMTP server is unavailable.");
                }

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:FromName"], _configuration["EmailSettings:FromEmail"]));
                emailMessage.To.Add(MailboxAddress.Parse(toEmail));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = message
                };

                if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
                {
                    bodyBuilder.Attachments.Add(attachmentPath);
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                await smtpClient.ConnectAsync(_configuration["EmailSettings:SmtpHost"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_configuration["EmailSettings:SmtpUser"], _configuration["EmailSettings:SmtpPass"]);
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with attachment to {Email}", toEmail);
                throw;
            }
        }
        public async Task SendEmailWithTemplateAsync(string toEmail, string subject, string templateName, Dictionary<string, string> parameters)
        {
            try
            {
                if (!await IsSmtpServerAvailableAsync())
                {
                    _logger.LogError("SMTP server is unavailable. Email cannot be sent.");
                    throw new InvalidOperationException("SMTP server is unavailable.");
                }

                var templateContent = await LoadTemplateAsync(templateName);

                foreach (var param in parameters)
                {
                    templateContent = templateContent.Replace($"{{{{{param.Key}}}}}", param.Value);
                }

                await SendEmailAsync(toEmail, subject, templateContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with template to {Email}", toEmail);
                throw;
            }
        }

        private async Task<string> LoadTemplateAsync(string templateName)
        {
            var templatePath = Path.Combine("Templates", $"{templateName}.html");
            return await File.ReadAllTextAsync(templatePath);
        }
        public async Task SendEmailWithRetryAsync(string toEmail, string subject, string message, int maxRetries = 3)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    await SendEmailAsync(toEmail, subject, message);
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    _logger.LogWarning(ex, "Attempt {RetryCount} failed for {Email}", retryCount, toEmail);
                    if (retryCount == maxRetries)
                    {
                        throw;
                    }
                    await Task.Delay(2000);
                }
            }
        }

    }
}
