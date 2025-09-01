using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rental.Domain.Applications.Services;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;
using Rental.Domain.Email.Validation;
using Rental.Domain.Maintenance.Services;

namespace Rental.Domain;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<EmailOptions>, EmailOptionsValidator>();
        services.AddOptionsWithValidateOnStart<EmailOptions>()
                .Bind(configuration.GetSection(EmailOptions.NAME));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMaintenanceRequestProcessor, MaintenanceRequestProcessor>();
        services.AddScoped<IRentalApplicationProcessor, RentalApplicationProcessor>();
        return services;
    }
}
