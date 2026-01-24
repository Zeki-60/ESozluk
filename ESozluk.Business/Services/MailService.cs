using ESozluk.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace ESozluk.Business.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendPasswordResetEmail(string toEmail, string resetToken)
        {
            // JSON dosyasındaki isme göre "MailSettings" olarak değiştirildi.
            // Eskiden kodda burası "EmailSettings" idi.
            var mailSettings = _configuration.GetSection("MailSettings");

            string htmlBody = $@"
            <html>
                <body>
                    <h2>Şifre Sıfırlama Talebi</h2>
                    <p>Şifrenizi sıfırlamak için aşağıdaki Token kodunu kullanınız:</p>
                    <h3 style='background-color: #eee; padding: 10px;'>{resetToken}</h3>
                    <p>Bu kod 1 saat süreyle geçerlidir.</p>
                </body>
            </html>";


            var email = new MimeMessage();
            // JSON'dan okuma kısımlarını düzelttik:
            email.From.Add(new MailboxAddress(mailSettings["DisplayName"], mailSettings["Mail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Şifre Sıfırlama";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };

            using var smtp = new SmtpClient();
            try
            {
                // JSON'dan host ve port okuma:
                smtp.Connect(
                    mailSettings["Host"],
                    int.Parse(mailSettings["Port"]),
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                // JSON'dan mail ve şifre okuma:
                smtp.Authenticate(mailSettings["Mail"], mailSettings["Password"]);

                smtp.Send(email);
            }
            catch (Exception ex)
            {
                // Hata olursa Swagger'da görebilmek için:
                throw new Exception($"Mail Gönderilemedi: {ex.Message}");
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }

        public void SendWelcomeEmail(string toEmail, string userName)
        {
            // ayarları json dan okuduk
            var mailSettings = _configuration.GetSection("MailSettings");


            var message = new MimeMessage();

            // Kimden
            message.From.Add(new MailboxAddress(mailSettings["DisplayName"], mailSettings["Mail"]));

            // Kime
            message.To.Add(new MailboxAddress(userName, toEmail));

            // Konu
            message.Subject = "ESozluk'e Hoş Geldin! 🚀";

            // İçerik:
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Merhaba {userName},\n\n" +
                                   "Aramıza katıldığın için çok mutluyuz. " +
                                   "Hemen ilk entry'ni girmeye ne dersin?\n\n" +
                                   "Sevgiler,\nESozluk Ekibi";



            message.Body = bodyBuilder.ToMessageBody();

            // MailKit ile Gönderme
            using (var client = new SmtpClient())
            {

                // Bağlan 
                client.Connect(mailSettings["Host"], int.Parse(mailSettings["Port"]), false);

                // Giriş Yap
                client.Authenticate(mailSettings["Mail"], mailSettings["Password"]);

                // Gönder
                client.Send(message);

                // Bağlantıyı Kes
                client.Disconnect(true);


            }
        }
    }
}