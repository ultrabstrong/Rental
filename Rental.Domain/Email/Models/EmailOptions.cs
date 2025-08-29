namespace Rental.Domain.Email.Models;

[Serializable]
public class EmailOptions
{
    public const string NAME = "EmailOptions";

    public string SMTPServer { get; set; } = string.Empty;

    public string SMTPUsername { get; set; } = string.Empty;

    public string SMTPPw { get; set; } = string.Empty;

    public int SMTPPort { get; set; }

    public string SMTPTo { get; set; } = string.Empty;
}
