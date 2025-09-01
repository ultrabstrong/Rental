using Rental.Domain.Validation;
using System.Text.RegularExpressions;

namespace Rental.UnitTests.Domain.Validation;

public class EmailValidationTests
{
    private static readonly Regex _regex = EmailValidation.EmailRegex();

    [Theory]
    // Basic + modern longer TLDs
    [InlineData("user@example.com")]
    [InlineData("USER.NAME_123@example-domain.com")]
    [InlineData("a.b-c_d@sub.domain.org")]
    [InlineData("abc@abc.io")] // 2-char tld
    [InlineData("abc@abc.tools")] // 5-char tld
    [InlineData("user@domain.comm")] // now valid (4-char)
    [InlineData("user@domain.company")] // long tld
    [InlineData("user@domain.corporate")] // long tld
    public void IsValid_ReturnsTrue_ForValidAddresses(string email)
    {
        Assert.Matches(_regex, email);
        Assert.True(EmailValidation.IsValid(email));
    }

    [Theory]
    // Null / empty / whitespace
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    // Structural issues
    [InlineData("plainaddress")]
    [InlineData("missingatsign.domain.com")]
    [InlineData("user@domain")] // no TLD
    [InlineData("user@domain.c")] // TLD too short (1)
    [InlineData("user@.com")] // domain starts with dot
    [InlineData(".user@domain.com")] // local starts with dot
    [InlineData("user.@domain.com")] // local ends with dot
    [InlineData("user..name@domain.com")] // double dot local
    [InlineData("user@domain..com")] // double dot domain
    public void IsValid_ReturnsFalse_ForInvalidAddresses(string? email)
    {
        if (email is not null)
        {
            Assert.DoesNotMatch(_regex, email);
        }
        Assert.False(EmailValidation.IsValid(email));
    }

    [Fact]
    public void Regex_DoesNotMatch_InvalidButSimilar()
    {
        Assert.DoesNotMatch(_regex, "user@domain.c"); // tld length 1
    }

    [Fact]
    public void Regex_Matches_ExpectedPattern()
    {
        Assert.Matches(_regex, "user.name-123@sub.domain.co");
    }
}
