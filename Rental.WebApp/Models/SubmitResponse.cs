using System.Text.Json.Serialization;

namespace Rental.WebApp.Models;

public class SubmitResponse
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("redirectUrl")]
    public string RedirectUrl { get; set; } = string.Empty;

    [JsonPropertyName("hasValidationErrors")]
    public bool HasValidationErrors { get; set; }
}