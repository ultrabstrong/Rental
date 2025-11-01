namespace Rental.WebApp.Services.HumanVerification;

public interface IHumanVerifier
{
    Task<bool> VerifyAsync(string token, string? remoteIp, CancellationToken ct = default);
}
