namespace Rental.WebApp.Models.Site;

public class SiteOptions
{
    public const string NAME = "SiteOptions";

    public string CompanyName { get; set; } = string.Empty;

    public string CompanyShortName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public Contact PostOffice { get; set; } = new Contact();

    public List<TenantInfoDoc> TenantInfoDocs { get; set; } = [];
}