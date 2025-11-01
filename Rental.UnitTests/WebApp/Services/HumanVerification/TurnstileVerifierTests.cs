using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Rental.WebApp.Services.HumanVerification;

namespace Rental.UnitTests.WebApp.Services.HumanVerification;

public class TurnstileVerifierTests
{
    [Fact]
    public async Task VerifyAsync_ReturnsTrue_WhenSecretNotConfigured()
    {
        // Arrange
        var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(handler.Object);
        var httpFactory = Mock.Of<IHttpClientFactory>(f =>
            f.CreateClient(TurnstileOptions.NAME) == httpClient
        );
        var options = Mock.Of<IOptionsSnapshot<TurnstileOptions>>(o =>
            o.Value == new TurnstileOptions { SecretKey = string.Empty }
        );
        ILogger<TurnstileVerifier> logger = NullLogger<TurnstileVerifier>.Instance;
        var sut = new TurnstileVerifier(httpFactory, options, logger);

        // Act
        var result = await sut.VerifyAsync("token", "127.0.0.1");

        // Assert
        Assert.True(result);
        handler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task VerifyAsync_ReturnsFalse_OnHttpFailure()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var handler = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);
        var httpClient = new HttpClient(handler.Object);
        var httpFactory = Mock.Of<IHttpClientFactory>(f =>
            f.CreateClient(TurnstileOptions.NAME) == httpClient
        );
        var options = Mock.Of<IOptionsSnapshot<TurnstileOptions>>(o =>
            o.Value == new TurnstileOptions { SecretKey = "secret" }
        );
        ILogger<TurnstileVerifier> logger = NullLogger<TurnstileVerifier>.Instance;
        var sut = new TurnstileVerifier(httpFactory, options, logger);

        // Act
        var result = await sut.VerifyAsync("token", "127.0.0.1");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task VerifyAsync_ParsesBody_ReturnsSuccessFlag(bool apiSuccess, bool expected)
    {
        // Arrange payload
        var payload = new { success = apiSuccess };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(payload),
        };
        var handler = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);
        var httpClient = new HttpClient(handler.Object);
        var httpFactory = Mock.Of<IHttpClientFactory>(f =>
            f.CreateClient(TurnstileOptions.NAME) == httpClient
        );
        var options = Mock.Of<IOptionsSnapshot<TurnstileOptions>>(o =>
            o.Value == new TurnstileOptions { SecretKey = "secret" }
        );
        ILogger<TurnstileVerifier> logger = NullLogger<TurnstileVerifier>.Instance;
        var sut = new TurnstileVerifier(httpFactory, options, logger);

        // Act
        var result = await sut.VerifyAsync("token", null);

        // Assert
        Assert.Equal(expected, result);
        // Verify request basic shape
        handler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(m =>
                    m.Method == HttpMethod.Post
                    && m.RequestUri!.ToString().Contains("/siteverify")
                    && m.Content is FormUrlEncodedContent
                ),
                ItExpr.IsAny<CancellationToken>()
            );
    }
}
