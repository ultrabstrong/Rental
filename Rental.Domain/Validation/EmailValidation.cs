using System.Text.RegularExpressions;

namespace Rental.Domain.Validation;

internal static partial class EmailValidation
{
    private static readonly Regex _emailRegex = EmailRegex();

    [GeneratedRegex(
        @"^(?!\.)(?!.*\.\.)([A-Za-z0-9_\-\.]+)(?<!\.)@(?:[A-Za-z0-9_\-]+\.)+[A-Za-z]{2,63}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    )]
    public static partial Regex EmailRegex();

    internal static bool IsValid(string? email) =>
        !string.IsNullOrWhiteSpace(email) && _emailRegex.IsMatch(email!);
}
