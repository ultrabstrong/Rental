using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;

namespace Rental.Domain;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.NAME));
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
