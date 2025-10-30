using System.Text.Json.Serialization;

namespace Rental.WebApp.Models;

public record SubmitResponse(
    [property: JsonPropertyName("isSuccess")] bool IsSuccess,
    [property: JsonPropertyName("redirectUrl")] string RedirectUrl = "",
    [property: JsonPropertyName("hasValidationErrors")] bool HasValidationErrors = false
);
