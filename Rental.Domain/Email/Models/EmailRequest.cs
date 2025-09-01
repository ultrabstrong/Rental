namespace Rental.Domain.Email.Models;

public record EmailRequest(
    string Subject,
    string Body,
    string AttachmentName,
    string PreferredReplyTo
);
