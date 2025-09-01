using Microsoft.Extensions.Options;
using Rental.Domain.Email.Models;
using Rental.Domain.Validation;

namespace Rental.Domain.Email.Validation;

internal sealed class EmailOptionsValidator : IValidateOptions<EmailOptions>
{
    public ValidateOptionsResult Validate(string? name, EmailOptions options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("Options instance is null");

        List<string> failures = [];

        if (string.IsNullOrWhiteSpace(options.SmtpServer))
            failures.Add($"{nameof(options.SmtpServer)} is required");
        if (!EmailValidation.IsValid(options.SmtpUsername))
            failures.Add($"{nameof(options.SmtpUsername)} must be a valid email address");
        if (string.IsNullOrWhiteSpace(options.SmtpPw))
            failures.Add($"{nameof(options.SmtpPw)} is required");
        if (options.SmtpPort < 1 || options.SmtpPort > 65535)
            failures.Add($"{nameof(options.SmtpPort)} must be between 1 and 65535");
        if (!EmailValidation.IsValid(options.SmtpTo))
            failures.Add($"{nameof(options.SmtpTo)} must be a valid email address");

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}
