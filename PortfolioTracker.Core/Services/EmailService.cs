using Microsoft.Extensions.Options;
using MimeKit;
using PortfolioTracker.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using PortfolioTracker.Core.Models;

namespace PortfolioTracker.Core.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        // improvements add exeption and methods for message and smtp...

        private MimeMessage BuildConfirmationMailMessage(string toEmail, string confirmationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            message.To.Add(new MailboxAddress("Test", toEmail));
            message.Subject = "Registration Link";
            message.Body = new TextPart("html")
            {
                Text = $"<a href=\"{confirmationLink}\">Please click on the registration link</a>"
            };
            return message;
        }
        private async Task SendMessageAsync(MimeMessage message)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        

        public async Task SendConfirmationEmailAsync(string toEmail, string confirmationLink)
        {
            try
            {
                var message = BuildConfirmationMailMessage(toEmail, confirmationLink);
                await SendMessageAsync(message);
            }
            catch (SmtpCommandException ex)
            {
                
                throw new EmailSendException($"Failed to send email to {toEmail}", ex);
            }
        }
    }
}
