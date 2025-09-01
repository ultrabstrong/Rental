namespace Rental.Domain.Email.Models;

public class EmailOptions
{
    public const string NAME = "EmailOptions";

    public string SmtpServer { get; init; } = string.Empty;
    public string SmtpUsername { get; init; } = string.Empty;
    public string SmtpPw { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SmtpTo { get; init; } = string.Empty;
}
