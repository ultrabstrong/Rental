using Microsoft.Extensions.Options;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Validation;
using Rental.UnitTests.ClassData;

namespace Rental.UnitTests.Domain.Email;

public class EmailOptionsValidatorTests
{
    private readonly EmailOptionsValidator _validator = new();

    private ValidateOptionsResult Validate(EmailOptions opts) => _validator.Validate(Options.DefaultName, opts);

    [Fact]
    public void Validate_ValidOptions_Succeeds()
    {
        var opts = new EmailOptions
        {
            SmtpServer = "smtp.example.com",
            SmtpUsername = "user@example.com",
            SmtpPw = "secret",
            SmtpPort = 587,
            SmtpTo = "dest@example.com"
        };

        var result = Validate(opts);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_MissingRequiredFields_FailsWithAllMessages()
    {
        var opts = new EmailOptions
        {
            SmtpServer = "", // missing
            SmtpUsername = "", // invalid / missing
            SmtpPw = "", // missing
            SmtpPort = 0, // invalid
            SmtpTo = "" // invalid / missing
        };

        var result = Validate(opts);

        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("SmtpServer is required", result.Failures);
        Assert.Contains("SmtpUsername must be a valid email address", result.Failures);
        Assert.Contains("SmtpPw is required", result.Failures);
        Assert.Contains("SmtpPort must be between 1 and 65535", result.Failures);
        Assert.Contains("SmtpTo must be a valid email address", result.Failures);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("userexample.com")] // missing @
    [InlineData("user@domain")] // no TLD
    [InlineData("user@.com")] // invalid domain
    public void Validate_InvalidEmails_Fails(string? badEmail)
    {
        var opts = new EmailOptions
        {
            SmtpServer = "smtp.example.com",
            SmtpUsername = badEmail ?? string.Empty,
            SmtpPw = "pw",
            SmtpPort = 25,
            SmtpTo = "dest@example.com"
        };

        var result = Validate(opts);

        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("SmtpUsername must be a valid email address", result.Failures);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(70000)]
    public void Validate_PortOutOfRange_Fails(int port)
    {
        var opts = new EmailOptions
        {
            SmtpServer = "smtp.example.com",
            SmtpUsername = "user@example.com",
            SmtpPw = "pw",
            SmtpPort = port,
            SmtpTo = "dest@example.com"
        };

        var result = Validate(opts);

        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("SmtpPort must be between 1 and 65535", result.Failures);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("userexample.com")] // missing @
    [InlineData("user@domain")] // no TLD
    [InlineData("user@.com")] // invalid domain
    public void Validate_InvalidRecipientEmail_Fails(string? badEmail)
    {
        var opts = new EmailOptions
        {
            SmtpServer = "smtp.example.com",
            SmtpUsername = "user@example.com",
            SmtpPw = "pw",
            SmtpPort = 587,
            SmtpTo = badEmail ?? string.Empty
        };

        var result = Validate(opts);

        Assert.False(result.Succeeded);
        Assert.NotNull(result.Failures);
        Assert.Contains("SmtpTo must be a valid email address", result.Failures);
    }
}
