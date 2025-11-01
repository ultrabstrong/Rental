using Rental.Domain.Defang;
using Rental.Domain.Maintenance.Models;

namespace Rental.UnitTests.Domain.Defang;

public class MaintenanceRequestDefangerTests
{
    const string DANGEROUS_INPUT = "<script>alert('xss')</script> http://example.com";
    const string SAFE_OUTPUT = "http[colon]//example[dot]com"; // tags stripped + defanged URL

    private readonly IDefanger _d = new WebInputDefanger();

    private MaintenanceRequestDefanger Create() => new(_d);

    [Fact]
    public void Defang_All_String_Fields_Are_Processed()
    {
        var sut = Create();
        var req = new MaintenanceRequest(
            RentalAddress: DANGEROUS_INPUT,
            FirstName: DANGEROUS_INPUT,
            LastName: DANGEROUS_INPUT,
            Email: DANGEROUS_INPUT,
            Phone: DANGEROUS_INPUT,
            Description: DANGEROUS_INPUT
        );

        var result = sut.Defang(req);

        Assert.Multiple(() =>
        {
            Assert.Equal(SAFE_OUTPUT, result.RentalAddress);
            Assert.Equal(SAFE_OUTPUT, result.FirstName);
            Assert.Equal(SAFE_OUTPUT, result.LastName);
            Assert.Equal(SAFE_OUTPUT, result.Email);
            Assert.Equal(SAFE_OUTPUT, result.Phone);
            Assert.Equal(SAFE_OUTPUT, result.Description);
        });
    }
}
