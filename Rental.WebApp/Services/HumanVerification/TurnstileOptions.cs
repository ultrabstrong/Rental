namespace Rental.WebApp.Services.HumanVerification;

public sealed class TurnstileOptions
{
    public const string NAME = "Turnstile";

    public string SiteKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
}
