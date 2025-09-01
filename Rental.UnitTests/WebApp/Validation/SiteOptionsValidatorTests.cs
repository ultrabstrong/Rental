using Microsoft.Extensions.Options;
using Rental.WebApp.Models.Site;
using Rental.WebApp.Validation;

namespace Rental.UnitTests.WebApp.Validation;

public class SiteOptionsValidatorTests
{
    private readonly SiteOptionsValidator _validator = new();

    private ValidateOptionsResult Validate(SiteOptions opts) => _validator.Validate(Options.DefaultName, opts);

    [Fact]
    public void Validate_AllValid_Succeeds()
    {
        var options = new SiteOptions
        {
            CompanyName = "Acme Property Management",
            CompanyShortName = "Acme",
            EmailAddress = "info@acme.test",
            PhoneNumber = "555-1234",
            Address = "1 Main St",
            PostOffice = new Contact { Address = "PO Box 42", Phone = "555-5678" },
            TenantInfoDocs = [new TenantInfoDoc { DisplayName = "Sample Doc", FileName = "sample.pdf" }]
        };

        var result = Validate(options);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_MissingRequiredTopLevel_Fails()
    {
        var options = new SiteOptions
        {
            CompanyName = "", // invalid
            CompanyShortName = null!, // invalid
            EmailAddress = " ", // invalid
            PhoneNumber = "", // invalid
            Address = null!, // invalid
            PostOffice = new Contact { Address = "", Phone = "" } // invalid fields
        };

        var result = Validate(options);

        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("CompanyName is required", result.Failures);
        Assert.Contains("CompanyShortName is required", result.Failures);
        Assert.Contains("EmailAddress is required", result.Failures);
        Assert.Contains("PhoneNumber is required", result.Failures);
        Assert.Contains("Address is required", result.Failures);
        Assert.Contains("PostOffice.Address is required", result.Failures);
        Assert.Contains("PostOffice.Phone is required", result.Failures);
    }

    [Fact]
    public void Validate_NullPostOffice_Fails()
    {
        var options = new SiteOptions
        {
            CompanyName = "Acme",
            CompanyShortName = "Acme",
            EmailAddress = "info@acme.test",
            PhoneNumber = "555-1234",
            Address = "1 Main St",
            PostOffice = null! // invalid
        };

        var result = Validate(options);
        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("PostOffice is required", result.Failures);
    }

    [Fact]
    public void Validate_TenantInfoDocs_NullEntry_Fails()
    {
        var options = new SiteOptions
        {
            CompanyName = "Acme",
            CompanyShortName = "Acme",
            EmailAddress = "info@acme.test",
            PhoneNumber = "555-1234",
            Address = "1 Main St",
            PostOffice = new Contact { Address = "PO Box 42", Phone = "555-5678" },
            TenantInfoDocs = [null!]
        };

        var result = Validate(options);
        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("TenantInfoDocs[0] must not be null", result.Failures);
    }

    [Fact]
    public void Validate_TenantInfoDocs_InvalidEntry_Fails()
    {
        var options = new SiteOptions
        {
            CompanyName = "Acme",
            CompanyShortName = "Acme",
            EmailAddress = "info@acme.test",
            PhoneNumber = "555-1234",
            Address = "1 Main St",
            PostOffice = new Contact { Address = "PO Box 42", Phone = "555-5678" },
            TenantInfoDocs = [
                new TenantInfoDoc { DisplayName = "", FileName = "doc.pdf" },
                new TenantInfoDoc { DisplayName = "Doc", FileName = "" }
            ]
        };

        var result = Validate(options);
        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("TenantInfoDocs[0].DisplayName is required", result.Failures);
        Assert.Contains("TenantInfoDocs[1].FileName is required", result.Failures);
    }
}
