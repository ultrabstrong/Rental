using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApartmentWeb.Models.Maintenance
{
    public class MaintenanceRequest : IEmailRequestBuilder
    {
        [Display(Name = "Rental Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the rental address")]
        public string RentalAddress { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone #")]
        public string Phone { get; set; }

        [Display(Name = "Please provide a brief summary of the maintenance needed")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a description of the maintenance needed")]
        public string Description { get; set; }

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
}
