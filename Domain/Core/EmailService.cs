using Domain.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Domain.Core
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly Regex _emailRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
        private readonly MailSettings _settings;
        private readonly SmtpClient _smtpClient;

        public EmailService(MailSettings settings)
        {
            _settings = settings;
            _smtpClient = new SmtpClient(settings.SMTPServer)
            {
                Port = settings.SMTPPort,
                Credentials = new NetworkCredential(settings.SMTPUsername, settings.SMTPPw),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public void SendEmail(IEmailRequestBuilder emailRequestBuilder, Stream toAttach)
        {
            var emailRequest = emailRequestBuilder.BuildEmailRequest();
            var attachment = new Attachment(toAttach, emailRequest.AttachmentName);

            MailMessage message = new MailMessage()
            {
                Subject = emailRequest.Subject,
                From = new MailAddress(_settings.SMTPUsername),
                Body = emailRequest.Body,
                IsBodyHtml = false,
                Attachments = { attachment },
                To = { _settings.SMTPTo }
            };

            if (_emailRegex.IsMatch(emailRequest.PreferredReplyTo))
            {
                message.ReplyToList.Clear();
                message.ReplyToList.Add(emailRequest.PreferredReplyTo);
            }

            _smtpClient.Send(message);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}
