using Domain.Models;
using System.IO;

namespace Domain.Core;

public interface IEmailService
{
    void SendEmail(IEmailRequestBuilder emailRequestBuilder, Stream toAttach);
}
