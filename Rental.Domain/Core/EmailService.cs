using Rental.Domain.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Domain.Core;

public class EmailService : IEmailService, IDisposable
{
    private readonly Regex _emailRegex = new(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
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

    public async Task SendEmailAsync(IEmailRequestBuilder emailRequestBuilder, Stream toAttach, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var message = BuildMailMessage(emailRequestBuilder, toAttach);
        // SmtpClient does not support CancellationToken directly
        await _smtpClient.SendMailAsync(message);
    }

    private MailMessage BuildMailMessage(IEmailRequestBuilder emailRequestBuilder, Stream toAttach)
    {
        var emailRequest = emailRequestBuilder.BuildEmailRequest();
        var attachment = new Attachment(toAttach, emailRequest.AttachmentName);

        var message = new MailMessage()
        {
            Subject = emailRequest.Subject,
            From = new MailAddress(_settings.SMTPUsername),
            Body = emailRequest.Body,
            IsBodyHtml = false
        };
        message.Attachments.Add(attachment);
        message.To.Add(_settings.SMTPTo);

        if (_emailRegex.IsMatch(emailRequest.PreferredReplyTo))
        {
            message.ReplyToList.Clear();
            message.ReplyToList.Add(emailRequest.PreferredReplyTo);
        }

        return message;
    }

    public void Dispose()
    {
        _smtpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
