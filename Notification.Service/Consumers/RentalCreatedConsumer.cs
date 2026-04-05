using EnterpriseTravelBooking.Shared.Events;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using MimeKit;

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
                _logger.LogInformation("[SMTP] Mail gönderimi başlatılıyor: {RentalId}", message.RentalId);

                var smtpHost = _configuration["Smtp:Host"];
                var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
                var smtpUser = _configuration["Smtp:Username"];
                var smtpPass = _configuration["Smtp:Password"];

                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("WanderSync Travel", "noreply@wander-sync.com"));
                mail.To.Add(MailboxAddress.Parse(message.CustomerEmail));
                mail.Subject = "Araç Kiralama Onayı";

                mail.Body = new TextPart("html")
                {
                    Text = $@"
                    <h3>Sayın Müşterimiz,</h3>
                    <p>Kiralama işleminiz başarıyla onaylanmıştır.</p>
                    <ul>
                        <li><b>Araç:</b> {message.VehicleInfo}</li>
                        <li><b>Toplam Tutar:</b> {message.TotalPrice:C2}</li>
                        <li><b>İşlem Tarihi:</b> {message.CreatedAt:dd.MM.yyyy HH:mm}</li>
                    </ul>
                    <p>Bizi tercih ettiğiniz için teşekkür ederiz!</p>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);

                _logger.LogInformation("[BAŞARILI] Mail {CustomerEmail} adresine ulaştı.", message.CustomerEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[HATA] Mail gönderilirken bir sorun oluştu.");
                throw;
            }
        }
    }
}
