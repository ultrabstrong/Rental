using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer.Core
{
    public class EmailService : IDisposable
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

        public void SendApplication(Application application, string applicationHtml, Stream applicationPdf)
        {
            var subject = $"Application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}; Co-Applicants : {application.OtherApplicants}";
            var body = $"Attached is the application for {application.RentalAddress} from {application.PersonalInfo.FirstName} {application.PersonalInfo.LastName}";
            var attachment = new Attachment(applicationPdf, $"{application.PersonalInfo.FirstName} {application.PersonalInfo.LastName} {application}.pdf");
            var preferredReplyTo = application.PersonalInfo.Email;

            SendEmail(subject, body, attachment, preferredReplyTo);
        }

        public void SendMaintenanceRequest(MaintenanceRequest maintenanceRequest, string applicationHtml, Stream maintenanceRequestPdf)
        {
            var subject = $"Maintenance request for {maintenanceRequest.RentalAddress} from {maintenanceRequest.FirstName} {maintenanceRequest.LastName}";

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"Attached is the maintenance request from {maintenanceRequest.FirstName} {maintenanceRequest.LastName} for {maintenanceRequest.RentalAddress}");
            bodyBuilder.AppendLine($"Email: {(string.IsNullOrWhiteSpace(maintenanceRequest.Email) ? "Not provided" : maintenanceRequest.Email)}");
            bodyBuilder.AppendLine($"Phone: {(string.IsNullOrWhiteSpace(maintenanceRequest.Phone) ? "Not provided" : maintenanceRequest.Phone)}");
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine(maintenanceRequest.Description);
            var body = bodyBuilder.ToString();

            var attachment = new Attachment(maintenanceRequestPdf, $"{maintenanceRequest.FirstName} {maintenanceRequest.LastName} Maintenance Request.pdf");
            var preferredReplyTo = maintenanceRequest.Email;

            SendEmail(subject, body, attachment, preferredReplyTo);
        }

        private void SendEmail(string subject, string body, Attachment attachment, string preferredReplyTo)
        {
            MailMessage message = new MailMessage()
            {
                Subject = subject,
                From = new MailAddress(_settings.SMTPUsername)
            };

            message.Attachments.Add(attachment);

            message.Body = body;
            message.IsBodyHtml = false;

#if DEBUG
            message.To.Add("ultrabstrong@gmail.com");
#else
            message.To.Add(_settings.SMTPTo);
#endif
            if (_emailRegex.IsMatch(preferredReplyTo))
            {
                message.ReplyToList.Clear();
                message.ReplyToList.Add(preferredReplyTo);
            }

            _smtpClient.Send(message);
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }
    }
}
