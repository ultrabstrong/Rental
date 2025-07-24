using Rental.Domain.Models;
using System.IO;

namespace Rental.Domain.Core;

public interface IEmailService
{
    void SendEmail(IEmailRequestBuilder emailRequestBuilder, Stream toAttach);
}
