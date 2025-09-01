using Microsoft.Extensions.Options;
using Rental.WebApp.Models.Site;

namespace Rental.WebApp.Validation;

internal sealed class SiteOptionsValidator : IValidateOptions<SiteOptions>
{
    public ValidateOptionsResult Validate(string? name, SiteOptions options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("Options instance is null");

        List<string> failures = [];

        // Required top-level string properties
        Require(options.CompanyName, nameof(options.CompanyName), failures);
        Require(options.CompanyShortName, nameof(options.CompanyShortName), failures);
        Require(options.EmailAddress, nameof(options.EmailAddress), failures);
        Require(options.PhoneNumber, nameof(options.PhoneNumber), failures);
        Require(options.Address, nameof(options.Address), failures);

        // PostOffice required and its properties required
        if (options.PostOffice is null)
        {
            failures.Add($"{nameof(options.PostOffice)} is required");
        }
        else
        {
            Require(options.PostOffice.Address, $"{nameof(options.PostOffice)}.{nameof(options.PostOffice.Address)}", failures);
            Require(options.PostOffice.Phone, $"{nameof(options.PostOffice)}.{nameof(options.PostOffice.Phone)}", failures);
        }

        // TenantInfoDocs optional, but if present validate each
        if (options.TenantInfoDocs is not null)
        {
            for (int i = 0; i < options.TenantInfoDocs.Count; i++)
            {
                var doc = options.TenantInfoDocs[i];
                if (doc is null)
                {
                    failures.Add($"TenantInfoDocs[{i}] must not be null");
                    continue;
                }
                Require(doc.DisplayName, $"TenantInfoDocs[{i}].{nameof(doc.DisplayName)}", failures);
                Require(doc.FileName, $"TenantInfoDocs[{i}].{nameof(doc.FileName)}", failures);
            }
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }

    private static void Require(string? value, string field, List<string> failures)
    {
        if (string.IsNullOrWhiteSpace(value))
            failures.Add($"{field} is required");
    }
}
