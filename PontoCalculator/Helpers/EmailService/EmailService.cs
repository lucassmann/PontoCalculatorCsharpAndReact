using MailKit.Net.Smtp;
using MimeKit;
using PontoCalculator.Dtos;

namespace PontoCalculator.Helpers.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("eden60@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(emailDto.To));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =
                $"<h1> Olá! </h1>" +
                $"<br>" +
                $"<h2> Use o código abaixo para criar uma nova senha no aplicativo </h2>" +
                $"<br>" +
                $"<h2>{emailDto.Token}</h2>"
            };
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        
        }
    }
}
