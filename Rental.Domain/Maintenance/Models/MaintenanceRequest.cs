using Rental.Domain.Email.Models;
using System.Text;

namespace Rental.Domain.Maintenance.Models;

public class MaintenanceRequest : IEmailRequestBuilder
{
    public string RentalAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public EmailRequest BuildEmailRequest()
    {
        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine($"Attached is the maintenance request from {FirstName} {LastName} for {RentalAddress}");
        bodyBuilder.AppendLine($"Email: {(string.IsNullOrWhiteSpace(Email) ? "Not provided" : Email)}");
        bodyBuilder.AppendLine($"Phone: {(string.IsNullOrWhiteSpace(Phone) ? "Not provided" : Phone)}");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine(Description);
        var body = bodyBuilder.ToString();


        return new EmailRequest()
        {
            Subject = $"Maintenance request for {RentalAddress} from {FirstName} {LastName}",
            Body = body,
            AttachmentName = $"{FirstName} {LastName} Maintenance Request.pdf",
            PreferredReplyTo = Email
        };
    }
}
