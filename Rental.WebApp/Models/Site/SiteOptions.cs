namespace Rental.WebApp.Models.Site;

public record SiteOptions
{
    public const string NAME = "SiteOptions";

    public string CompanyName { get; init; } = string.Empty;

    public string CompanyShortName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public Contact PostOffice { get; init; } = new Contact();

    public List<TenantInfoDoc> TenantInfoDocs { get; init; } = [];
}
