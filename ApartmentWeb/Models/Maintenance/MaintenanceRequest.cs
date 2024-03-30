using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using rm = Resources.WebsiteModels.MaintenanceRequest;
using vrm = Resources.WebsiteModels.MaintenanceValidation;

namespace ApartmentWeb.Models.Maintenance
{
    public class MaintenanceRequest : IEmailRequestBuilder
    {
        [Display(Name = nameof(rm.MAINTENANCE_RENTAL_ADDRESS), ResourceType = typeof(rm))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.MAINTENANCE_RENTAL_ADDRESS), ErrorMessageResourceType = typeof(vrm))]
        public string RentalAddress { get; set; }

        [Display(Name = nameof(rm.MAINTENANCE_FIRSTNAME), ResourceType = typeof(rm))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.MAINTENANCE_FIRSTNAME), ErrorMessageResourceType = typeof(vrm))]
        public string FirstName { get; set; }

        [Display(Name = nameof(rm.MAINTENANCE_LASTNAME), ResourceType = typeof(rm))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.MAINTENANCE_LASTNAME), ErrorMessageResourceType = typeof(vrm))]
        public string LastName { get; set; }

        [Display(Name = nameof(rm.MAINTENANCE_EMAIL), ResourceType = typeof(rm))]
        public string Email { get; set; }

        [Display(Name = nameof(rm.MAINTENANCE_PHONE), ResourceType = typeof(rm))]
        public string Phone { get; set; }

        [Display(Name = nameof(rm.MAINTENANCE_DESCRIPTION), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.MAINTENANCE_DESCRIPTION), ErrorMessageResourceType = typeof(vrm))]
        public string Description { get; set; }

        public EmailRequest BuildEmailRequest()
        {
            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine($"Attached is the maintenance request from {FirstName}   {LastName}  for  {RentalAddress}");
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
