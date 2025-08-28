using Rental.Domain.Models;

namespace Rental.WebApp.Models.Site;

public class SiteDetails
{
    public string CompanyName { get; set; } = string.Empty;

    public string CompanyShortName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Contact PostOffice { get; set; } = new Contact();

    public List<TenantInfoDoc> TenantInfoDocs { get; set; } = [];

    public MailSettings MailSettings { get; set; } = new MailSettings();

}