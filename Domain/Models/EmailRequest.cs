using System.IO;

namespace Domain.Models
{
    public class EmailRequest
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public Stream ToAttach { get; set; }

        public string AttachmentName { get; set; }

        public string PreferredReplyTo { get; set; }
    }
}
