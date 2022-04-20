using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Threading.Tasks;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Model.Mail;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Template.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }




        public async Task<bool> SendMail(Email email)
        {
            if (_emailSettings.UseDevServer)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("template","from_address@example.com"));
                emailMessage.To.Add(MailboxAddress.Parse(email.To));
                emailMessage.Subject =email.Subject;
                emailMessage.Body = new TextPart(TextFormat.Plain) { Text = email.Body };

                // send email
                using var devClient = new SmtpClient();
                var secureSocketOption = SecureSocketOptions.None;
                devClient.Connect("localhost", 25, secureSocketOption);
                await devClient.SendAsync(emailMessage);
                await devClient.DisconnectAsync(true);
                
                return true;

            }
            var client = new SendGridClient(_emailSettings.ApiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var body = email.Body;

            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(sendGridMessage);

            return response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> SendRegistrationMail(string address, string url)
        {
            var email = new Email
            {
                To = address,
                Subject = "Email confirmation",
                Body = $"<p> To finalize your registration click <a href=\"{url}\">here</a>. :) </p>"
            };
            var succeeded =  await SendMail(email);
            return succeeded;
        }
        public async Task<bool> SendForgotPasswordMail(string address, string url)
        {
            var email = new Email
            {
                To = address,
                Subject = "Password Reset",
                Body = $"<p> To reset your password click <a href=\"{url}\">here</a>.</p>"
            };
            return await SendMail(email);
        }

    }
}
