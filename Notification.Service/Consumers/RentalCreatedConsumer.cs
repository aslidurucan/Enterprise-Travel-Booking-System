using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Service.Consumers
{
    public class RentalCreatedConsumer : IConsumer<RentalCreatedEvent>
    {
        private readonly ILogger<RentalCreatedConsumer> _logger;
        private readonly IConfiguration _configuration;

        public RentalCreatedConsumer(ILogger<RentalCreatedConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<RentalCreatedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation($"[SMTP] Mail gönderimi başlatılıyor: {message.RentalId}");

                var smtpHost = _configuration["Smtp:Host"];
                var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
                var smtpUser = _configuration["Smtp:Username"];
                var smtpPass = _configuration["Smtp:Password"];

               
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@wander-sync.com", "WanderSync Travel"),
                    Subject = "Araç Kiralama Onayı 🚗",
                    Body = $@"
                    <h3>Sayın Müşterimiz,</h3>
                    <p>Kiralama işleminiz başarıyla onaylanmıştır.</p>
                    <ul>
                        <li><b>Araç:</b> {message.VehicleInfo}</li>
                        <li><b>Toplam Tutar:</b> {message.TotalPrice:C2}</li>
                        <li><b>İşlem Tarihi:</b> {message.CreatedAt:dd.MM.yyyy HH:mm}</li>
                    </ul>
                    <p>Bizi tercih ettiğiniz için teşekkür ederiz!</p>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(message.CustomerEmail);

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation($"[BAŞARILI] Mail {message.CustomerEmail} adresine ulaştı.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[HATA] Mail gönderilirken bir sorun oluştu: {ex.Message}");
            }
        }
    }
}
