using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace RentACar.WebUI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var settings = _config.GetSection("EmailSettings");

                var client = new SmtpClient(settings["SmtpHost"])
                {
                    Port = int.Parse(settings["SmtpPort"]),
                    Credentials = new NetworkCredential(settings["SenderEmail"], settings["Password"]),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings["SenderEmail"], settings["SenderName"]),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Mail gönderildi: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mail gönderilemedi: {Email}", email);
            }
        }
    }
}