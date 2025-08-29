using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Rental.Domain.Applications.Models;
using Rental.Domain.Applications.Services;
using Rental.Domain.Email.Models;
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
        _emailService.Verify(e => e.SendEmailAsync(It.IsAny<EmailRequest>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_BuildsExpectedEmailRequest()
    {
        var application = new RentalApplication
        {
            RentalAddress = "456 Example Ave",
            OtherApplicants = "John Smith",
            PersonalInfo = new PersonalInfo { FirstName = "Alice", LastName = "Johnson", Email = "alice@example.com" }
        };
        var pdfBytes = new byte[] {4,5,6};
        _pdfService.Setup(p => p.GenerateAsync(It.IsAny<RentalApplication>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pdfBytes);
        EmailRequest? captured = null;
        _emailService.Setup(e => e.SendEmailAsync(It.IsAny<EmailRequest>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<EmailRequest,Stream,CancellationToken>((req,_,_) => captured = req)
            .Returns(Task.CompletedTask);

        var processor = new TestProcessor(_pdfService.Object, _emailService.Object, _logger.Object);
        await processor.InvokeProcessAsync(application, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.Equal("Application for 456 Example Ave from Alice Johnson; Co-Applicants: John Smith", captured!.Subject);
        Assert.Equal("Alice Johnson Application.pdf", captured.AttachmentName);
        Assert.Equal("alice@example.com", captured.PreferredReplyTo);
        Assert.Contains("456 Example Ave", captured.Body);
        Assert.Contains("Alice Johnson", captured.Body);
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
        _emailService.Verify(e => e.SendEmailAsync(It.IsAny<EmailRequest>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
