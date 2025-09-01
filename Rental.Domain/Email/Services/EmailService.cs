using Microsoft.Extensions.Options;
using Rental.Domain.Email.Models;
using System.Net;
using System.Net.Mail;
using Rental.Domain.Validation;

namespace Rental.Domain.Email.Services;

internal class EmailService : IEmailService, IDisposable
{
    private readonly EmailOptions _emailOptions;
    private readonly SmtpClient _smtpClient;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
        _smtpClient = new SmtpClient(_emailOptions.SmtpServer)
        {
            Port = _emailOptions.SmtpPort,
            Credentials = new NetworkCredential(_emailOptions.SmtpUsername, _emailOptions.SmtpPw),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
    }

    async Task IEmailService.SendEmailAsync(EmailRequest emailRequest, Stream attachmentStream, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var message = BuildMailMessage(emailRequest, attachmentStream);
        await _smtpClient.SendMailAsync(message, cancellationToken);
    }

    private MailMessage BuildMailMessage(EmailRequest emailRequest, Stream toAttach)
    {
        var attachment = new Attachment(toAttach, emailRequest.AttachmentName);

        var message = new MailMessage()
        {
            Subject = emailRequest.Subject,
            From = new MailAddress(_emailOptions.SmtpUsername),
            Body = emailRequest.Body,
            IsBodyHtml = false
        };
        message.Attachments.Add(attachment);
        message.To.Add(_emailOptions.SmtpTo);

        if (EmailValidation.IsValid(emailRequest.PreferredReplyTo))
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
