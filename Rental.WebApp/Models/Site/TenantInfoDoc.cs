namespace Rental.WebApp.Models.Site;

public record TenantInfoDoc
{
    public string DisplayName { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
}
