namespace Rental.Domain.Models;

[Serializable]
public class MailSettings
{
    public string SMTPServer { get; set; } = string.Empty;

    public string SMTPUsername { get; set; } = string.Empty;

    public string SMTPPw { get; set; } = string.Empty;

    public int SMTPPort { get; set; }

    public string SMTPTo { get; set; } = string.Empty;
}
