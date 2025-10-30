using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rental.Domain.Applications.Services;
using Rental.Domain.Defang;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;
using Rental.Domain.Email.Validation;
using Rental.Domain.Maintenance.Services;

namespace Rental.Domain;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<IServiceProvider, IRentalApplicationPdfService> rentalApplicationPdfFactory,
        Func<IServiceProvider, IMaintenanceRequestPdfService> maintenanceRequestPdfFactory
    )
    {
        services.AddSingleton<IValidateOptions<EmailOptions>, EmailOptionsValidator>();
        services
            .AddOptionsWithValidateOnStart<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.NAME));

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMaintenanceRequestProcessor, MaintenanceRequestProcessor>();
        services.AddScoped<IRentalApplicationProcessor, RentalApplicationProcessor>();

        // Defanging
        services.AddSingleton<IDefanger, WebInputDefanger>();
        services.AddScoped<IMaintenanceRequestDefanger, MaintenanceRequestDefanger>();

        // Per-scope creation using provided factories
        services.AddScoped(rentalApplicationPdfFactory);
        services.AddScoped(maintenanceRequestPdfFactory);

        return services;
    }
}
