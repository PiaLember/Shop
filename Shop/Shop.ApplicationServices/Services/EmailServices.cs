using Microsoft.Extensions.Configuration;
using MimeKit;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using MailKit.Net.Smtp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace Shop.ApplicationServices.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            if (dto.AttachmentPaths != null && dto.AttachmentPaths.Any())
            {
                foreach (var path in dto.AttachmentPaths)
                {
                    bodyBuilder.Attachments.Add(path);
                }
            }

            email.Body = bodyBuilder.ToMessageBody();


            //kindlasti kasutada mailkit.net.smtp
            using var smtp = new SmtpClient();

            smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
