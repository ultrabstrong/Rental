using System.Text.Json.Serialization;
using Corely.Common.Http;
using Microsoft.Extensions.Options;

namespace Rental.WebApp.Services.HumanVerification;

internal sealed class TurnstileVerifier : IHumanVerifier
{
    private readonly HttpClient _http;
    private readonly IOptionsSnapshot<TurnstileOptions> _options;
    private readonly ILogger<TurnstileVerifier> _logger;

    public TurnstileVerifier(
        IHttpClientFactory httpFactory,
        IOptionsSnapshot<TurnstileOptions> options,
        ILogger<TurnstileVerifier> logger
    )
    {
        _http = httpFactory.CreateClient(TurnstileOptions.NAME);
        _options = options;
        _logger = logger;
    }

    public async Task<bool> VerifyAsync(
        string token,
        string? remoteIp,
        CancellationToken ct = default
    )
    {
        var secret = _options.Value.SecretKey;
        if (string.IsNullOrWhiteSpace(secret))
        {
            _logger.LogWarning("Turnstile secret key not configured; skipping verification");
            return true; // treat as pass when not configured to avoid blocking in dev
        }

        var form = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["secret"] = secret,
                ["response"] = token ?? string.Empty,
                ["remoteip"] = remoteIp ?? string.Empty,
            }
        );

        // Build an HttpRequestMessage so middleware can attach logging extensions
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://challenges.cloudflare.com/turnstile/v0/siteverify"
        )
        {
            Content = form,
        };
        request.Headers.Accept.ParseAdd("application/json");

        request.EnableRequestResponseDetailLogging();

        using var resp = await _http.SendAsync(request, ct);
        if (!resp.IsSuccessStatusCode)
            return false;

        var payload = await resp.Content.ReadFromJsonAsync<TurnstileResponse>(
            cancellationToken: ct
        );
        return payload?.Success == true;
    }

    private sealed class TurnstileResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; set; }
    }
}
