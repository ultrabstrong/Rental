using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Rental.Domain.Applications.Models;
using Rental.Domain.Applications.Services;
using Rental.Domain.Email.Services;

namespace Rental.UnitTests.Domain.Services;

public class RentalApplicationProcessorTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IRentalApplicationPdfService> _pdfService = new();
    private readonly Mock<IEmailService> _emailService = new();
    private readonly Mock<ILogger<RentalApplicationProcessor>> _logger = new();

    private class TestProcessor : RentalApplicationProcessor
    {
        public TestProcessor(IRentalApplicationPdfService p, IEmailService e, ILogger<RentalApplicationProcessor> l) : base(p,e,l) {}
        public Task InvokeProcessAsync(RentalApplication app, CancellationToken ct) => ProcessAsync(app, ct);
    }

    [Fact]
    public async Task ProcessAsync_GeneratesPdfAndSendsEmail()
    {
        var application = _fixture.Create<RentalApplication>();
        var pdfBytes = _fixture.Create<byte[]>();
        _pdfService.Setup(p => p.GenerateAsync(application, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pdfBytes);

        var processor = new TestProcessor(_pdfService.Object, _emailService.Object, _logger.Object);
        await processor.InvokeProcessAsync(application, CancellationToken.None);

        _pdfService.Verify(p => p.GenerateAsync(application, It.IsAny<CancellationToken>()), Times.Once);
        _emailService.Verify(e => e.SendEmailAsync(application, It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_CancellationRequested_Throws()
    {
        var application = _fixture.Create<RentalApplication>();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var processor = new TestProcessor(_pdfService.Object, _emailService.Object, _logger.Object);

        await Assert.ThrowsAsync<OperationCanceledException>(() => processor.InvokeProcessAsync(application, cts.Token));
        _pdfService.Verify(p => p.GenerateAsync(It.IsAny<RentalApplication>(), It.IsAny<CancellationToken>()), Times.Never);
        _emailService.Verify(e => e.SendEmailAsync(It.IsAny<RentalApplication>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
