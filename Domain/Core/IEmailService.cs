using Domain.Models;
using System.IO;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IEmailService
    {
        Task SendEmailAsync(IEmailRequestBuilder emailRequestBuilder, Stream toAttach);
    }
}
