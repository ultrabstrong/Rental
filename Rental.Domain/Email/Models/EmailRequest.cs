namespace Rental.Domain.Email.Models;

public class EmailRequest
{
    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string AttachmentName { get; set; } = string.Empty;

    public string PreferredReplyTo { get; set; } = string.Empty;
}
