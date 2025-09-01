namespace Rental.WebApp.Models.Site;

public record Contact
{
    public string Address { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}
