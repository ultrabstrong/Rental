using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Rental.Domain.Defang;
using Rental.Domain.Email.Models;
using Rental.Domain.Email.Services;
using Rental.Domain.Maintenance.Models;
using Rental.Domain.Maintenance.Services;

namespace Rental.UnitTests.Domain.Services;

public class MaintenanceRequestProcessorTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IMaintenanceRequestPdfService> _pdfService = new();
    private readonly Mock<IEmailService> _emailService = new();
    private readonly Mock<ILogger<MaintenanceRequestProcessor>> _logger = new();
    private readonly IMaintenanceRequestDefanger _defanger = new MaintenanceRequestDefanger(
        new WebInputDefanger()
    );

    private class TestProcessor : MaintenanceRequestProcessor
    {
        public TestProcessor(
            IMaintenanceRequestPdfService p,
            IEmailService e,
            ILogger<MaintenanceRequestProcessor> l,
            IMaintenanceRequestDefanger s
        )
            : base(p, e, l, s) { }

        public Task InvokeHandleAsync(MaintenanceRequest req, CancellationToken ct) =>
            ((IMaintenanceRequestProcessor)this).HandleAsync(req, ct);
    }

    [Fact]
    public async Task HandleAsync_GeneratesPdfAndSendsEmail()
    {
        var maintenanceRequest = _fixture.Create<MaintenanceRequest>();
        var pdfBytes = _fixture.Create<byte[]>();
        _pdfService
            .Setup(p =>
                p.GenerateAsync(It.IsAny<MaintenanceRequest>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(pdfBytes);

        var processor = new TestProcessor(
            _pdfService.Object,
            _emailService.Object,
            _logger.Object,
            _defanger
        );
        await processor.InvokeHandleAsync(maintenanceRequest, CancellationToken.None);

        _pdfService.Verify(
            p => p.GenerateAsync(It.IsAny<MaintenanceRequest>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        _emailService.Verify(
            e =>
                e.SendEmailAsync(
                    It.IsAny<EmailRequest>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task HandleAsync_BuildsExpectedEmailRequest()
    {
        var maintenanceRequest = new MaintenanceRequest(
            "123 Test St",
            "Jane",
            "Doe",
            "jane.doe@example.com",
            "555-1111",
            "Sink is leaking"
        );
        var pdfBytes = new byte[] { 1, 2, 3 };
        _pdfService
            .Setup(p =>
                p.GenerateAsync(It.IsAny<MaintenanceRequest>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(pdfBytes);
        EmailRequest? captured = null;
        _emailService
            .Setup(e =>
                e.SendEmailAsync(
                    It.IsAny<EmailRequest>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .Callback<EmailRequest, Stream, CancellationToken>((req, _, _) => captured = req)
            .Returns(Task.CompletedTask);

        var processor = new TestProcessor(
            _pdfService.Object,
            _emailService.Object,
            _logger.Object,
            _defanger
        );
        await processor.InvokeHandleAsync(maintenanceRequest, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal("Maintenance request for 123 Test St from Jane Doe", captured!.Subject);
        Assert.Equal("Jane Doe Maintenance Request.pdf", captured.AttachmentName);
        Assert.Equal("jane.doe@example.com", captured.PreferredReplyTo);
        Assert.Contains("Sink is leaking", captured.Body);
        Assert.Contains("Email:", captured.Body);
        Assert.Contains("Phone:", captured.Body);
    }

    [Fact]
    public async Task HandleAsync_CallsDefanger_AndUsesResult()
    {
        var dirty = new MaintenanceRequest("DIRTY", "A", "B", "C", "D", "E");
        var safe = dirty with { RentalAddress = "SAFE" };
        var defangerMock = new Mock<IMaintenanceRequestDefanger>();
        defangerMock.Setup(d => d.Defang(dirty)).Returns(safe);
        var pdfBytes = _fixture.Create<byte[]>();
        _pdfService
            .Setup(p =>
                p.GenerateAsync(It.IsAny<MaintenanceRequest>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(pdfBytes);

        var processor = new TestProcessor(
            _pdfService.Object,
            _emailService.Object,
            _logger.Object,
            defangerMock.Object
        );
        await processor.InvokeHandleAsync(dirty, CancellationToken.None);

        defangerMock.Verify(d => d.Defang(dirty), Times.Once);
        _pdfService.Verify(
            p =>
                p.GenerateAsync(
                    It.Is<MaintenanceRequest>(r => r.RentalAddress == "SAFE"),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task HandleAsync_CancellationRequested_Throws()
    {
        var maintenanceRequest = _fixture.Create<MaintenanceRequest>();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var processor = new TestProcessor(
            _pdfService.Object,
            _emailService.Object,
            _logger.Object,
            _defanger
        );

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            processor.InvokeHandleAsync(maintenanceRequest, cts.Token)
        );
        _pdfService.Verify(
            p => p.GenerateAsync(It.IsAny<MaintenanceRequest>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        _emailService.Verify(
            e =>
                e.SendEmailAsync(
                    It.IsAny<EmailRequest>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }
}
